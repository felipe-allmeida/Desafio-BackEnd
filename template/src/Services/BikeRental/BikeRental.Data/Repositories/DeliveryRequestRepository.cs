using BikeRental.Domain.Models.DeliveryRequestAggregate;
using BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.Repositories
{
    public class DeliveryRequestRepository : IDeliveryRequestRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<DeliveryRequest> _set;
        public DeliveryRequestRepository(BikeRentalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<DeliveryRequest>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public DeliveryRequest Add(DeliveryRequest entity)
        {
            if (entity.IsTransient())
            {
                return _set.Add(entity).Entity;

            }
            return entity;
        }

        public void Delete(DeliveryRequest entity)
        {
            _set.Remove(entity);
        }

        public async Task<DeliveryRequest?> GetByIdAsync(Guid id)
        {
            return await _set.SingleOrDefaultAsync(x => x.Id == id);
        }

        public void Update(DeliveryRequest entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }

}
