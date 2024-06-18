using BikeRental.Domain.Models.DeliveryRequestNotificationAggregate;
using BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.Repositories
{
    public class DeliveryRequestNotificationRepository : IDeliveryRequestNotificationRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<DeliveryRequestNotification> _set;

        public DeliveryRequestNotificationRepository(BikeRentalContext context)
        {
            _context = context;
            _set = context.Set<DeliveryRequestNotification>();
        }
        public IUnitOfWork UnitOfWork => _context;

        public DeliveryRequestNotification Add(DeliveryRequestNotification entity)
        {
            if (entity.IsTransient())
            {
                return _set.Add(entity).Entity;

            }
            return entity;
        }

        public void Delete(DeliveryRequestNotification entity)
        {
            _set.Remove(entity);
        }

        public Task<DeliveryRequestNotification?> GetDeliveryRequestNotification(Guid deliveryRequestId, long deliveryRiderId)
        {
            return _set.SingleOrDefaultAsync(x => x.DeliveryRequestId == deliveryRequestId && x.DeliveryRiderId == deliveryRiderId);
        }

        public void Update(DeliveryRequestNotification entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }

}
