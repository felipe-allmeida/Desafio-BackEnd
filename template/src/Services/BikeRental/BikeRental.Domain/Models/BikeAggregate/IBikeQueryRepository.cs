using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.BikeAggregate
{
    public interface IBikeQueryRepository : IQueryRepository<Bike>
    {
        Task<Bike?> GetFirstAvailableBikeAsync(DateTimeOffset startAt, DateTimeOffset endAt);
    }
}
