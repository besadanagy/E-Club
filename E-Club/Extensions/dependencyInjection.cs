
using Hangfire.SqlServer;

namespace E_Club.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddControllers();

            services
                .AddSwaggerServices()
                .AddMapsterConfigurations()
                .AddFluentValidationsConfigurations()
                .AddAuthConfigurations(configuration)
                .AddCorsConfiguration(configuration)
                .AddHangfireConfigurations(configuration);

            // هذا حل مؤقت للـ Migration - نستخدم واجهة وهمية
            services.AddScoped<IEmailSender,MailSenderServices>(); // أو أي class بسيط ينفذ IEmailSender
            services.AddScoped<IDashboardService, DashboardService>();

            // Database Context
            services.AddDatabaseConfiguration(configuration);

            // Email Settings
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddScoped<IEmailSender, MailSenderServices>();
            // HttpContext Accessor
            services.AddHttpContextAccessor();

            // Dependency Injection for Services
            services.AddApplicationServices();

            return services;
        }

        private static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "E-Club API",
                    Version = "v1",
                    Description = "E-Club Management System API"
                });

                // Add JWT Authentication to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }

        private static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var allowOrigins = configuration.GetSection("AllowOrigins").Get<string[]>();

            // Default values if not found in config
            if (allowOrigins == null || allowOrigins.Length == 0)
            {
                allowOrigins = new[] { "http://localhost:4200", "http://localhost:3000" };
            }

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins(allowOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });

                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }

        private static IServiceCollection AddMapsterConfigurations(this IServiceCollection services)
        {
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;
        }

        private static IServiceCollection AddFluentValidationsConfigurations(this IServiceCollection services)
        {
            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddAuthConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                // User settings
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add Authorization Handlers
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            // Configure JWT Options
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            if (jwtSettings == null)
                throw new InvalidOperationException("JWT settings are not configured properly.");

            // Add JWT Provider
            services.AddSingleton<IJwtProvider, JwtProvider>();

            // Add Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Handle token in cookies or headers
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        private static IServiceCollection AddHangfireConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            // استخدم DefaultConnection لو HangfireConnection مش موجودة
            var hangfireConnection = configuration.GetConnectionString("HangfireConnection")
                ?? configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(hangfireConnection))
                throw new InvalidOperationException("No connection string found for Hangfire.");

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(hangfireConnection, new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = true,  // دي بتخلق الجداول لو مش موجودة
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(15)
                }));

            services.AddHangfireServer();

            return services;
        }
    }
}
// ضيفي ده في أي مكان (مثلاً، تحت usings مباشرة)
public class DummyEmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
        => Task.CompletedTask; // بيعمل حاجة 
}