using BikeRental.Domain.Models.BikeAggregate;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.QueryRepositories
{
    public class BikeQueryRepository : IBikeQueryRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<Bike> _set;

        public BikeQueryRepository(BikeRentalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<Bike>();
        }

        public IQueryable<Bike> GetAll()
        {
            return _set.AsQueryable().AsNoTracking();
        }

        public async Task<Bike?> GetFirstAvailableBikeAsync(DateTimeOffset startAt, DateTimeOffset endAt)
        {
            var startAtUtc = startAt.ToUniversalTime();
            var endAtUtc = endAt.ToUniversalTime();

            return await _set                
                .Where(x => !x.Rentals.Any(r => r.StartAt <= endAtUtc && r.EndAt >= startAtUtc))
                .FirstOrDefaultAsync();
        }
    }
}
