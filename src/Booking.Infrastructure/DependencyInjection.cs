using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Booking.Application.Abstractions.Clocks;
using Booking.Infrastructure.Clocks;
using Booking.Application.Abstractions.Email;
using Booking.Infrastructure.Email;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Booking.Application.Abstractions.Repositories;
using Booking.Infrastructure.Repositories;
using Booking.Application.Abstractions.Data;
using Dapper;
using Booking.Infrastructure.Converters;

namespace Booking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork>(config => config.GetRequiredService<AppDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            return services;
        }
    }
}
