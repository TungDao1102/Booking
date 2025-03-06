using Booking.API.Middleware;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }

        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        //public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
        //{
        //    app.UseMiddleware<RequestContextLoggingMiddleware>();
        //    return app;
        //}
    }
}
