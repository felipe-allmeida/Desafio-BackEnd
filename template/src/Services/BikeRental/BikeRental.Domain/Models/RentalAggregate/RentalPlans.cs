namespace BikeRental.Domain.Models.RentalAggregate
{
    public static class RentalPlans
    {
        public static readonly Dictionary<int, int> DailyPricesByPeriod = new Dictionary<int, int>
        {
            { 7, 3000 },
            { 15, 2800 },
            { 30, 2200 }
        };

        public static readonly Dictionary<int, decimal> DaysInAdvancePenaltiesByPeriod = new Dictionary<int, decimal>
        {
            { 7, 0.20m },
            { 15, 0.40m },
            { 30, 0.60m }
        };

        public static readonly int DaysDelayedPenaltyPriceCents = 5000;
    }
}
