using BikeRental.Domain.Events;
using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.DeliveryRequestAggregate;
using BikeRental.Domain.Models.RentalAggregate;
using BikeRental.Domain.ValueObjects;
using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.DeliveryRiderAggregate
{
    public class DeliveryRider : AggregateRoot<long>
    {
        private readonly List<Rental> _rentals;
        private readonly List<DeliveryRequest> _deliveryRequests;

        protected DeliveryRider() : base()
        {
            _rentals = [];
            _deliveryRequests = [];
        }

        public DeliveryRider(string userId, string name, DateTimeOffset birthday, CNPJ cnpj) : this()
        {
            UserId = userId;
            Name = name;
            UpdateBirthday(birthday);
            UpdateCNPJ(cnpj);

            AddDomainEvent(new DeliveryRiderCreatedDomainEvent(this));
        }

        public string UserId { get; private set; }
        public string Name { get; private set; }
        public CNPJ Cnpj { get; private set; }
        public CNH? Cnh { get; private set; }
        public long? CurrentBikeId { get; private set; }
        public Guid? CurrentDeliveryRequestId { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTimeOffset Birthday { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }
        public virtual IReadOnlyList<Rental> Rentals => _rentals.AsReadOnly();
        public virtual IReadOnlyList<DeliveryRequest> DeliveryRequests => _deliveryRequests.AsReadOnly();

        public void UpdateCNPJ(CNPJ cnpj)
        {
            Cnpj = cnpj;            
        }

        public void UpdateCNH(CNH cnh)
        {
            Cnh = cnh;
        }

        public void UpdateBirthday(DateTimeOffset birthday)
        {
            var age = (DateTimeOffset.UtcNow.Year - birthday.Year);
            if (age < 18)
            {
                throw new DomainException("The rider must be at least 18 years old.");
            }
            Birthday = birthday;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
        }
        
        public void AttachBike(long bikeId)
        {
            if (CurrentBikeId.HasValue)
                throw new DomainException("The rider already has a bike.");

            CurrentBikeId = bikeId;
        }

        public void DetachBike()
        {
            if (!CurrentBikeId.HasValue)
                throw new DomainException("The rider does not have a bike.");

            CurrentBikeId = null;
        }

        public void AcceptDeliveryRequestId(Guid deliveryRequestId)
        {
            if (CurrentDeliveryRequestId.HasValue)
                throw new DomainException("The rider already has a delivery request.");

            CurrentDeliveryRequestId = deliveryRequestId;
        }

        public void FinishDeliveryRequest()
        {
            if (!CurrentDeliveryRequestId.HasValue)
                throw new DomainException("The rider does not have a delivery request.");

            CurrentDeliveryRequestId = null;
        }
    }
}
