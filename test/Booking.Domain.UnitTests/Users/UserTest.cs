using Booking.Domain.UnitTests.Commons;
using Booking.Domain.Users;
using Booking.Domain.Users.Authorizations;
using Booking.Domain.Users.Events;
using FluentAssertions;

namespace Booking.Domain.UnitTests.Users
{
    public class UserTest : BaseTest
    {
        [Fact]
        public void Create_Should_SetPropertiesValues()
        {
            var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
            user.FirstName.Should().Be(UserData.FirstName);
            user.LastName.Should().Be(UserData.LastName);
            user.Email.Should().Be(UserData.Email);
        }

        [Fact]
        public void Create_Should_RaiseUserCreatedDomainEvent()
        {
            var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
            UserCreatedEvent userCreatedDomainEvent = AssertDomainEventWasPublished<UserCreatedEvent>(user);
            userCreatedDomainEvent.UserId.Should().Be(user.Id);
        }

        [Fact]
        public void Create_Should_AddRegisteredRoleToUser()
        {
            var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
            user.Roles.Should().Contain(Role.Registered);
        }
    }
}
