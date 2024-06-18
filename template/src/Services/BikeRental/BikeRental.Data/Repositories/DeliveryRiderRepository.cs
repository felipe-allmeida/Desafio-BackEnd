using BikeRental.Domain.Models.DeliveryRiderAggregate;
using BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.Repositories
{
    public class DeliveryRiderRepository : IDeliveryRiderRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<DeliveryRider> _set;

        public DeliveryRiderRepository(BikeRentalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<DeliveryRider>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public DeliveryRider Add(DeliveryRider entity)
        {
            if (entity.IsTransient())
            {
                return _set.Add(entity).Entity;
            }

            return entity;
        }

        public void Delete(DeliveryRider entity)
        {
            _set.Remove(entity);
        }

        public Task<bool> ExistsByCnhAsync(string cnh)
        {
            return _set.AsNoTracking().AnyAsync(x => x.Cnh.Number == cnh);
        }

        public Task<bool> ExistsByCnpjAsync(string cnpj)
        {
            return _set.AsNoTracking().AnyAsync(x => x.Cnpj.Value == cnpj);
        }

        public async Task<DeliveryRider?> GetByIdAsync(long id)
        {
            return await _set.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<DeliveryRider?> GetByUserIdAsync(string userId)
        {
            return await _set.SingleOrDefaultAsync(x => x.UserId == userId);
        }

        public void Update(DeliveryRider entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
