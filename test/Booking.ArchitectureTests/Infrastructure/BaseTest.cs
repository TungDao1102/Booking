using Booking.API;
using Booking.Application;
using Booking.Domain;
using Booking.Infrastructure;
using System.Reflection;

namespace Booking.ArchitectureTests.Infrastructure
{

    public abstract class BaseTest
    {
        protected static readonly Assembly ApplicationAssembly = typeof(IApplicationAssemblyMarker).Assembly;

        protected static readonly Assembly DomainAssembly = typeof(IDomainAssemblyMarker).Assembly;

        protected static readonly Assembly InfrastructureAssembly = typeof(IInfrastructureAssemblyMarker).Assembly;

        protected static readonly Assembly PresentationAssembly = typeof(IApiAssemblyMarker).Assembly;
    }
}
