using BikeRental.Domain.Models.DeliveryRiderAggregate;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.QueryRepositories
{
    public class DeliveryRiderQueryRepository : IDeliveryRiderQueryRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<DeliveryRider> _set;

        public DeliveryRiderQueryRepository(BikeRentalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<DeliveryRider>();
        }

        public IQueryable<DeliveryRider> GetAll()
        {
            return _set.AsQueryable().AsNoTracking();
        }

        public async Task<List<DeliveryRider>> GetAvaiableDeliveryRiders()
        {
            return await _set
                .Where(x => x.CurrentBikeId != null && x.CurrentDeliveryRequestId == null)
                .ToListAsync();
        }
    }
}
