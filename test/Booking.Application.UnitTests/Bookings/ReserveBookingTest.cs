using Booking.Application.Abstractions.Clocks;
using Booking.Application.Abstractions.Repositories;
using Booking.Application.Bookings.ReserveBooking;
using Booking.Application.Exceptions;
using Booking.Application.UnitTests.Apartments;
using Booking.Application.UnitTests.Users;
using Booking.Domain.Apartments;
using Booking.Domain.Bookings;
using Booking.Domain.Commons;
using Booking.Domain.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Booking.Application.UnitTests.Bookings
{
    public class ReserveBookingTest
    {
        private static readonly DateTime UtcNow = DateTime.UtcNow;
        private static readonly ReserveBookingCommand Command = new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateOnly(2024, 1, 1),
            new DateOnly(2024, 1, 10));

        private readonly ReserveBookingCommandHandler _handler;
        private readonly IUserRepository _userRepositoryMock;
        private readonly IApartmentRepository _apartmentRepositoryMock;
        private readonly IBookingRepository _bookingRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public ReserveBookingTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _apartmentRepositoryMock = Substitute.For<IApartmentRepository>();
            _bookingRepositoryMock = Substitute.For<IBookingRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            IDateTimeProvider dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
            dateTimeProviderMock.UtcNow.Returns(UtcNow);

            _handler = new ReserveBookingCommandHandler(
                _userRepositoryMock,
                _apartmentRepositoryMock,
                _bookingRepositoryMock,
                _unitOfWorkMock,
                new PricingService(),
                dateTimeProviderMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUserIsNull()
        {
            _userRepositoryMock
                .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns((User?)null);

            Result<Guid> result = await _handler.Handle(Command, default);
            result.Error.Should().Be(UserErrors.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenApartmentIsNull()
        {
            User user = UserData.Create();

            _userRepositoryMock
                .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock
                .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
                .Returns((Apartment?)null);

            Result<Guid> result = await _handler.Handle(Command, default);
            result.Error.Should().Be(ApartmentErrors.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenApartmentIsBooked()
        {
            User user = UserData.Create();
            Apartment apartment = ApartmentData.Create();
            var duration = DateRange.Create(Command.DateStart, Command.DateEnd);

            _userRepositoryMock
                .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock
                .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock
                .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
                .Returns(true);

            Result<Guid> result = await _handler.Handle(Command, default);
            result.Error.Should().Be(BookingErrors.Overlap);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkThrows()
        {
            User user = UserData.Create();
            Apartment apartment = ApartmentData.Create();
            var duration = DateRange.Create(Command.DateStart, Command.DateEnd);

            _userRepositoryMock
                .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock
                .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock
                .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
                .Returns(false);

            _unitOfWorkMock
                .SaveChangesAsync()
                .ThrowsAsync(new ConcurrencyException("Concurrency", new Exception()));

            Result<Guid> result = await _handler.Handle(Command, default);
            result.Error.Should().Be(BookingErrors.Overlap);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenBookingIsReserved()
        {
            User user = UserData.Create();
            Apartment apartment = ApartmentData.Create();
            var duration = DateRange.Create(Command.DateStart, Command.DateEnd);

            _userRepositoryMock
                .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock
                .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock
                .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
                .Returns(false);

            Result<Guid> result = await _handler.Handle(Command, default);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenBookingIsReserved()
        {
            User user = UserData.Create();
            Apartment apartment = ApartmentData.Create();
            var duration = DateRange.Create(Command.DateStart, Command.DateEnd);

            _userRepositoryMock
                .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock
                .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
                .Returns(apartment);
            _bookingRepositoryMock
                .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
                .Returns(false);

            Result<Guid> result = await _handler.Handle(Command, default);
            _bookingRepositoryMock.Received(1).Add(Arg.Is<Domain.Bookings.Booking>(b => b.Id == result.Value));
        }
    }
}
