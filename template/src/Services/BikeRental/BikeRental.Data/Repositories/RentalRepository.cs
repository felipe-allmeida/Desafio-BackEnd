using BikeRental.Domain.Models.RentalAggregate;
using BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<Rental> _set;

        public RentalRepository(BikeRentalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<Rental>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public Rental Add(Rental entity)
        {
            if (entity.IsTransient())
            {
                return _set.Add(entity).Entity;
            }

            return entity;
        }

        public void Delete(Rental entity)
        {
            _set.Remove(entity);
        }

        public async Task<bool> ExistsRentalForBike(long bikeId)
        {
            return await _set.AnyAsync(x => x.BikeId == bikeId);
        }

        public async Task<Rental?> GetByIdAsync(long deliveryRiderId, Guid id)
        {
            return await _set.SingleOrDefaultAsync(x => x.Id == id && x.DeliveryRiderId == deliveryRiderId);
        }

        public void Update(Rental entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
