﻿using Booking.Domain.Abstractions;
using Booking.Domain.Apartments;
using Booking.Domain.Bookings.Events;
using Booking.Domain.Commons;

namespace Booking.Domain.Bookings
{
    public sealed class Booking : BaseEntity
    {
        private Booking()
        {
        }
        private Booking(Guid id, Guid apartmentId, Guid userId, DateRange duration, Money priceForPeriod, Money cleaningFee, Money amenitiesUpCharge, Money totalPrice, BookingStatus status, DateTime createdOn)
        : base(id)
        {
            ApartmentId = apartmentId;
            UserId = userId;
            Duration = duration;
            PriceForPeriod = priceForPeriod;
            CleaningFee = cleaningFee;
            AmenitiesUpCharge = amenitiesUpCharge;
            TotalPrice = totalPrice;
            Status = status;
            CreatedOn = createdOn;
        }

        public Guid ApartmentId { get; private set; }
        public Guid UserId { get; private set; }
        public DateRange Duration { get; private set; } = default!;
        public Money PriceForPeriod { get; private set; } = default!;
        public Money CleaningFee { get; private set; } = default!;
        public Money AmenitiesUpCharge { get; private set; } = default!;
        public Money TotalPrice { get; private set; } = default!;
        public BookingStatus Status { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? ConfirmedOn { get; private set; }
        public DateTime? RejectedOn { get; private set; }
        public DateTime? CompletedOn { get; private set; }
        public DateTime? CancelledOn { get; private set; }

        public static Booking Reserve(Apartment apartment, Guid userId, DateRange duration, DateTime dateTime, PricingService pricingService)
        {
            PricingDetails pricingDetails = pricingService.CalculatePrice(apartment, duration);

            var booking = new Booking(
                Guid.NewGuid(),
                apartment.Id,
                userId,
                duration,
                pricingDetails.PriceForPeriod,
                pricingDetails.CleaningFee,
                pricingDetails.AmenitiesUpCharge,
                pricingDetails.TotalPrice,
                BookingStatus.Reserved,
                dateTime);

            booking.AddDomainEvent(new BookingReservedDomainEvent(booking.Id));

            apartment.LastBooked = dateTime;

            return booking;
        }

        public Result Confirm(DateTime dateTime)
        {
            if (Status != BookingStatus.Reserved)
            {
                return Result.Failure(BookingErrors.NotReserved);
            }

            Status = BookingStatus.Confirmed;
            ConfirmedOn = dateTime;

            AddDomainEvent(new BookingConfirmedDomainEvent(Id));

            return Result.Success();
        }

        public Result Reject(DateTime dateTime)
        {
            if (Status != BookingStatus.Reserved)
            {
                return Result.Failure(BookingErrors.NotReserved);
            }

            Status = BookingStatus.Rejected;
            RejectedOn = dateTime;

            AddDomainEvent(new BookingRejectedDomainEvent(Id));

            return Result.Success();
        }

        public Result Complete(DateTime dateTime)
        {
            if (Status != BookingStatus.Confirmed)
            {
                return Result.Failure(BookingErrors.NotConfirmed);
            }

            Status = BookingStatus.Completed;
            CompletedOn = dateTime;

            AddDomainEvent(new BookingCompletedDomainEvent(Id));

            return Result.Success();
        }

        public Result Cancel(DateTime dateTime)
        {
            if (Status != BookingStatus.Confirmed)
            {
                return Result.Failure(BookingErrors.NotConfirmed);
            }

            var currentDate = DateOnly.FromDateTime(dateTime);

            if (currentDate > Duration.Start)
            {
                return Result.Failure(BookingErrors.AlreadyStarted);
            }

            Status = BookingStatus.Cancelled;
            CancelledOn = dateTime;

            AddDomainEvent(new BookingCancelledDomainEvent(Id));

            return Result.Success();
        }
    }
}
