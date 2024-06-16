using BikeRental.Domain.Models.BikeAggregate;
using BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<Bike> _bikeSet;
        
        public IUnitOfWork UnitOfWork => _context;
        public BikeRepository(BikeRentalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _bikeSet = context.Set<Bike>();
        }

        public Bike Add(Bike entity)
        {
            if (entity.IsTransient())
            {
                return _bikeSet.Add(entity).Entity;
            }

            return entity;
        }

        public void Delete(Bike entity)
        {
            _bikeSet.Remove(entity);
        }

        public Task<bool> ExistsByPlateAsync(string plate)
        {
            return _bikeSet.AsNoTracking().AnyAsync(x => x.Plate == plate);
        }

        public async Task<Bike> GetByIdAsync(long id)
        {
            return await _bikeSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Bike entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
