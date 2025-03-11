using Asp.Versioning;
using Booking.Application.Abstractions.Authentications;
using Booking.Application.Abstractions.Caching;
using Booking.Application.Abstractions.Clocks;
using Booking.Application.Abstractions.Data;
using Booking.Application.Abstractions.Email;
using Booking.Application.Abstractions.Repositories;
using Booking.Infrastructure.Authentications;
using Booking.Infrastructure.Authorizations;
using Booking.Infrastructure.Caching;
using Booking.Infrastructure.Clocks;
using Booking.Infrastructure.Converters;
using Booking.Infrastructure.Data;
using Booking.Infrastructure.Email;
using Booking.Infrastructure.HealthChecks;
using Booking.Infrastructure.Repositories;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Booking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            AddPersistence(services, configuration);
            AddAuthentication(services, configuration);
            AddAuthorization(services);
            AddCaching(services, configuration);
            AddHealthChecks(services, configuration);
            AddApiVersioning(services);

            return services;
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<AppDbContext>(options =>
            {
                // EFCore.NamingConventions
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork>(config => config.GetRequiredService<AppDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.Configure<AuthenticationOption>(configuration.GetSection("Authentication"));

            services.ConfigureOptions<JwtBearerOptionsSetup>();
            services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));
            services.AddTransient<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthService, AuthService>((serviceProvider, httpClient) =>
            {
                var keyCloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            }).AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
            {
                var keyCloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                httpClient.BaseAddress = new Uri(keyCloakOptions.TokenUrl);
            });

            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext, UserContext>();
        }

        private static void AddAuthorization(IServiceCollection services)
        {
            services.AddScoped<AuthorizationService>();
            services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        }

        private static void AddCaching(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Cache") ??
                                      throw new ArgumentNullException(nameof(configuration));

            services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

            services.AddSingleton<ICacheService, CacheService>();
        }

        private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck<CustomSqlHealthCheck>("custom-sql")
                .AddNpgSql(configuration.GetConnectionString("Database")!)
                .AddRedis(configuration.GetConnectionString("Cache")!)
                .AddUrlGroup(new Uri(configuration["KeyCloak:BaseUrl"]!), HttpMethod.Get, "keycloak");
        }

        private static void AddApiVersioning(IServiceCollection services)
        {
            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1);
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddMvc()
                .AddApiExplorer(options =>
                {
                    // for swagger
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}
