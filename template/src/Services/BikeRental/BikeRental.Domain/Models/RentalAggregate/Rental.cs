using BikeRental.Domain.Enums;
using BikeRental.Domain.Events;
using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.BikeAggregate;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.RentalAggregate
{
    public class Rental : AggregateRoot<Guid>
    {
        public Rental() : base()
        {

        }

        public Rental(DateTimeOffset startAt, DateTimeOffset endAt, DateTimeOffset expectedReturnAt) : this()
        {
            if (startAt.Date > endAt.Date)
                throw new DomainException("StartAt must be before EndAt.");

            if (startAt.Date > expectedReturnAt.Date)
                throw new DomainException("StartAt must be before ExpectedReturnAt.");

            StartAt = startAt.ToUniversalTime();
            EndAt = endAt.ToUniversalTime();
            ExpectedReturnAt = expectedReturnAt.ToUniversalTime();

            CalculatePrice();
        }

        public Rental(long bikeId, long deliveryRiderId, DateTimeOffset startAt, DateTimeOffset endAt, DateTimeOffset expectedReturnAt) 
            : this(startAt, endAt, expectedReturnAt)
        {
            BikeId = bikeId;
            DeliveryRiderId = deliveryRiderId;
            Status = ERentalStatus.Pending;

            AddDomainEvent(new BikeRentedDomainEvent(this));
        }

        public long BikeId { get; private set; }
        public long DeliveryRiderId { get; private set; }
        public ERentalStatus Status { get; private set; }

        public DateTimeOffset StartAt { get; private set; }
        public DateTimeOffset EndAt { get; private set; }
        public DateTimeOffset ExpectedReturnAt { get; private set; }

        public int DailyPriceCents { get; private set; }
        public int PriceCents { get; private set; }
        public int? PenaltyPriceCents { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }
        public virtual Bike Bike { get; private set; }
        public virtual DeliveryRider DeliveryRider { get; private set; }

        public void MarkAsInProgress()
        {
            if (Status != ERentalStatus.Pending)
                throw new DomainException("Rental must be in Pending status to be marked as InProgress.");
            Status = ERentalStatus.InProgress;
            AddDomainEvent(new RentalInProgressDomainEvent(this));
        }

        public void MarkAsCompleted()
        {
            if (Status != ERentalStatus.InProgress)
                throw new DomainException("Rental must be in InProgress status to be marked as Completed.");
            Status = ERentalStatus.Completed;
            AddDomainEvent(new RentalCompletedDomainEvent(this));
        }

        private void CalculatePrice()
        {
            var totalDays = (EndAt - StartAt).Days;
            var expectedTotalDays = (ExpectedReturnAt - StartAt).Days;

            // Ensure the selected totalDays has a defined price
            if (!RentalPlans.DailyPricesByPeriod.ContainsKey(totalDays))
                throw new DomainException($"No daily price defined for {totalDays} days. Rental period must be 7, 15 or 30 days.");

            // Get the daily price in cents based on the rental period
            DailyPriceCents = RentalPlans.DailyPricesByPeriod[totalDays];

            // Calculate the total price based on the expected total days
            PriceCents = DailyPriceCents * expectedTotalDays;

            // Calculate penalties if the ExpectedReturnAt is different from the EndAt
            if (ExpectedReturnAt.Date < EndAt.Date)
            {
                var daysInAdvance = (EndAt - ExpectedReturnAt).Days;

                // Calculate the penalty based on the days in advance
                PenaltyPriceCents = (int)Math.Round((daysInAdvance * DailyPriceCents) * RentalPlans.DaysInAdvancePenaltiesByPeriod[totalDays]);

                // Add the penalty to the price
                PriceCents += PenaltyPriceCents.Value;
            }
            else if (ExpectedReturnAt.Date > EndAt.Date)
            {
                var daysDelayed = (ExpectedReturnAt - EndAt).Days;

                // Calculate the additional value for the extra days (R$50 per day)
                PenaltyPriceCents = daysDelayed * RentalPlans.DaysDelayedPenaltyPriceCents;

                // Add the penalty to the price
                PriceCents += PenaltyPriceCents.Value;
            }
        }
    }
}
