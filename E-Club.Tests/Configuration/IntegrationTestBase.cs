using E_Club.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Xunit;

namespace E_Club.Tests.Configuration;

public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    protected readonly ApplicationDbContext DbContext;
    protected readonly CustomWebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;

    protected IntegrationTestBase(CustomWebApplicationFactory<Program> factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // إعداد HttpContext وهمي عشان الـ SaveChangesAsync تاخد الـ TestUserId
        var httpContextAccessor = _scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, TestUserId)
        }, "TestAuth"));

        httpContextAccessor.HttpContext = new DefaultHttpContext { User = user };
    }

    protected T GetService<T>() where T : notnull
    {
        return _scope.ServiceProvider.GetRequiredService<T>();
    }

    // ID الـ Test User الوهمي — بيتستخدم في كل الـ Tests
    protected const string TestUserId = "test-user-id-12345";

    public Task InitializeAsync()
    {
        // الـ Schema اتعمل بالفعل في CustomWebApplicationFactory
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }
}
