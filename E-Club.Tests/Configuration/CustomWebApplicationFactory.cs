using E_Club.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace E_Club.Tests.Configuration;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private static readonly SqliteConnection _keepAliveConnection;
    private const string ConnectionString = "DataSource=EClubTest;Mode=Memory;Cache=Shared;Foreign Keys=False";

    static CustomWebApplicationFactory()
    {
        _keepAliveConnection = new SqliteConnection(ConnectionString);
        _keepAliveConnection.Open();

        using var cmd = _keepAliveConnection.CreateCommand();
        cmd.CommandText = "PRAGMA foreign_keys = OFF;";
        cmd.ExecuteNonQuery();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(ConnectionString)
            .Options;

        var httpContextAccessor = new HttpContextAccessor();
        using var context = new ApplicationDbContext(options, httpContextAccessor);
        context.Database.EnsureCreated();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // إزالة إعدادات SQL Server
            var optionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (optionsDescriptor != null) services.Remove(optionsDescriptor);

            var efInternalDescriptors = services
                .Where(d => d.ServiceType.FullName != null &&
                            d.ServiceType.FullName.Contains("IDbContextOptionsConfiguration") &&
                            d.ServiceType.FullName.Contains("ApplicationDbContext"))
                .ToList();
            foreach (var d in efInternalDescriptors)
                services.Remove(d);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(ConnectionString);
                options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning));
            });

            // إضافة Authentication وهمي للـ System Tests
            // بنجبر السيستم يستخدم الـ Test scheme كافتراضي
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
                options.DefaultScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        });

        builder.UseEnvironment("Testing");
    }
}

// Handler وهمي بيمرر أي Request كأنه Admin مسجل دخول
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { 
            new Claim(ClaimTypes.NameIdentifier, "test-user-id-12345"),
            new Claim(ClaimTypes.Role, "Admin") 
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
