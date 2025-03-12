using Booking.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Application.IntegrationTests.Infrastructure
{
    [Collection("Api collection")]
    public abstract class ApiCollection : IClassFixture<ApiFixture>
    {
        private readonly IServiceScope _scope;
        protected readonly ISender Sender;
        protected readonly AppDbContext DbContext;
        protected ApiCollection(ApiFixture fixture)
        {
            _scope = fixture.Services.CreateScope();

            Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
            DbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }
    }
}
