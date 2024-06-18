using BikeRental.Domain.Events;
using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.RentalAggregate;
using BuildingBlocks.Common;
using System.Text.RegularExpressions;

namespace BikeRental.Domain.Models.BikeAggregate
{
    public class Bike : AggregateRoot<long>
    {
        private readonly Regex _platePattern = new Regex("^(?=(?:[^A-Za-z]*[A-Za-z]){4}[^A-Za-z]*$)(?=(?:[^0-9]*[0-9]){3}[^0-9]*$)[A-Za-z0-9]{7}$");
        private readonly List<Rental> _rentals;

        protected Bike() : base()
        {
            _rentals = [];
        }

        public Bike(string plate, int year, string model) : this()
        {
            Year = year;
            Model = model;

            UpdatePlate(plate);

            AddDomainEvent(new BikeCreatedDomainEvent(this));
        }

        public string Plate { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt {get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }
        public virtual IReadOnlyList<Rental> Rentals => _rentals.AsReadOnly();

        public void UpdatePlate(string plate)
        {
            if (!_platePattern.IsMatch(plate))
                throw new DomainException("Invalid plate");

            Plate = plate;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
        }
    }
}
