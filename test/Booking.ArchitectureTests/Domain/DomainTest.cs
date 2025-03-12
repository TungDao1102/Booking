using Booking.ArchitectureTests.Infrastructure;
using Booking.Domain.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;

namespace Booking.ArchitectureTests.Domain
{
    public class DomainTest : BaseTest
    {
        [Fact]
        public void DomainEvents_Should_BeSealed()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
