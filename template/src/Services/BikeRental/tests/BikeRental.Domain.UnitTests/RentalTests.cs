using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.RentalAggregate;

namespace BikeRental.Domain.UnitTests
{
    public class RentalTests
    {
        [Fact]
        public void CalculatePrice_7DaysRental_NoPenalty()
        {
            // Arrange
            var startAt = DateTimeOffset.UtcNow.AddDays(1);
            var endAt = startAt.AddDays(7);
            var expectedReturnAt = endAt;

            // Act
            var rental = new Rental(startAt, endAt, expectedReturnAt);

            // Assert
            Assert.Equal(3000, rental.DailyPriceCents);
            Assert.Equal(7 * 3000, rental.PriceCents);
            Assert.Null(rental.PenaltyPriceCents);
        }

        [Fact]
        public void CalculatePrice_15DaysRental_NoPenalty()
        {
            // Arrange
            var startAt = DateTimeOffset.UtcNow.AddDays(1);
            var endAt = startAt.AddDays(15);
            var expectedReturnAt = endAt;

            // Act
            var rental = new Rental(startAt, endAt, expectedReturnAt);

            // Assert
            Assert.Equal(2800, rental.DailyPriceCents);
            Assert.Equal(15 * 2800, rental.PriceCents);
            Assert.Null(rental.PenaltyPriceCents);
        }

        [Fact]
        public void CalculatePrice_30DaysRental_NoPenalty()
        {
            // Arrange
            var startAt = DateTimeOffset.UtcNow.AddDays(1);
            var endAt = startAt.AddDays(30);
            var expectedReturnAt = endAt;

            // Act
            var rental = new Rental(startAt, endAt, expectedReturnAt);

            // Assert
            Assert.Equal(2200, rental.DailyPriceCents);
            Assert.Equal(30 * 2200, rental.PriceCents);
            Assert.Null(rental.PenaltyPriceCents);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]        
        public void CalculatePrice_7DaysRental_NDaysPenalty(int days)
        {
            // Arrange
            var startAt = DateTimeOffset.UtcNow.AddDays(1);
            var endAt = startAt.AddDays(7);
            var expectedReturnAt = startAt.AddDays(days);
            
            var daysInAdvance = (endAt - expectedReturnAt).Days;

            // Act
            var rental = new Rental(startAt, endAt, expectedReturnAt);

            // Assert
            var penalty = (int)Math.Round((daysInAdvance * 3000) * 0.20m);
            var expectedPrice = ((expectedReturnAt - startAt).Days * 3000) + penalty;

            Assert.Equal(3000, rental.DailyPriceCents);
            Assert.Equal(expectedPrice, rental.PriceCents);
            Assert.Equal(penalty, rental.PenaltyPriceCents);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        public void CalculatePrice_15DaysRental_NDaysPenalty(int days)
        {
            // Arrange
            var startAt = DateTimeOffset.UtcNow.AddDays(1);
            var endAt = startAt.AddDays(15);
            var expectedReturnAt = startAt.AddDays(days);

            var daysInAdvance = (endAt - expectedReturnAt).Days;

            // Act
            var rental = new Rental(startAt, endAt, expectedReturnAt);

            // Assert
            var penalty = (int)Math.Round((daysInAdvance * 2800) * 0.40m);
            var expectedPrice = ((expectedReturnAt - startAt).Days * 2800) + penalty;

            Assert.Equal(2800, rental.DailyPriceCents);
            Assert.Equal(expectedPrice, rental.PriceCents);
            Assert.Equal(penalty, rental.PenaltyPriceCents);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(18)]
        [InlineData(19)]
        [InlineData(20)]
        [InlineData(21)]
        [InlineData(22)]
        [InlineData(23)]
        [InlineData(24)]
        [InlineData(25)]
        [InlineData(26)]
        [InlineData(27)]
        [InlineData(28)]
        [InlineData(29)]
        public void CalculatePrice_30DaysRental_NDaysPenalty(int days)
        {
            // Arrange
            var startAt = DateTimeOffset.UtcNow.AddDays(1);
            var endAt = startAt.AddDays(30);
            var expectedReturnAt = startAt.AddDays(days);

            var daysInAdvance = (endAt - expectedReturnAt).Days;

            // Act
            var rental = new Rental(startAt, endAt, expectedReturnAt);

            // Assert
            var penalty = (int)Math.Round((daysInAdvance * 2200) * 0.60m);
            var expectedPrice = ((expectedReturnAt - startAt).Days * 2200) + penalty;

            Assert.Equal(2200, rental.DailyPriceCents);
            Assert.Equal(expectedPrice, rental.PriceCents);
            Assert.Equal(penalty, rental.PenaltyPriceCents);
        }

        [Theory]
        [InlineData(7, 1)]
        [InlineData(7, 5)]
        [InlineData(7, 10)]        
        [InlineData(15, 1)]
        [InlineData(15, 5)]
        [InlineData(15, 10)]
        [InlineData(30, 1)]
        [InlineData(30, 5)]
        [InlineData(30, 10)]
        public void CalculatePrice_NDaysRental_ExpectedReturnDateAfterEndAt(int days, int daysDelayed)
        {
            // Arrange
            var startAt = DateTimeOffset.UtcNow.AddDays(1);
            var endAt = startAt.AddDays(days);
            var expectedReturnAt = endAt.AddDays(daysDelayed);

            // Act
            var rental = new Rental(startAt, endAt, expectedReturnAt);

            // Assert
            Assert.Equal(5000 * daysDelayed, rental.PenaltyPriceCents);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(8)]
        [InlineData(14)]
        [InlineData(16)]
        [InlineData(29)]
        [InlineData(31)]
        public void CalculatePrice_InvalidRentalPeriod_ExceptionThrown(int days)
        {
            // Arrange
            var startAt = DateTimeOffset.UtcNow.AddDays(1);
            var endAt = startAt.AddDays(days);
            var expectedReturnAt = endAt.AddDays(days);

            // Act
            // Assert 
            Assert.Throws<DomainException>(() => new Rental(startAt, endAt, expectedReturnAt));
        }

        [Fact]
        public void CalculatePrice_InvalidExpectedReturnDate_ExceptionThrown()
        {
            // Arrange
            var startAt = DateTimeOffset.UtcNow.AddDays(1);
            var endAt = startAt.AddDays(7);
            var expectedReturnAt = startAt.AddDays(-1);

            // Act
            // Assert 
            Assert.Throws<DomainException>(() => new Rental(startAt, endAt, expectedReturnAt));
        }
    }
}