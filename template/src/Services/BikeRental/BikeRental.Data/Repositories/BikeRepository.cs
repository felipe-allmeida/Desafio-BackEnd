using BikeRental.Domain.Models.BikeAggregate;
using BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<Bike> _set;
        
        public BikeRepository(BikeRentalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<Bike>();
        }
        public IUnitOfWork UnitOfWork => _context;

        public Bike Add(Bike entity)
        {
            if (entity.IsTransient())
            {
                return _set.Add(entity).Entity;
            }

            return entity;
        }

        public Task<bool> BikeHasRentals(long id)
        {
            return _set.AsNoTracking().AnyAsync(x => x.Id == id && x.Rentals.Any());
        }

        public void Delete(Bike entity)
        {
            _set.Remove(entity);
        }

        public Task<bool> ExistsByPlateAsync(string plate)
        {
            return _set.AsNoTracking().AnyAsync(x => x.Plate == plate);
        }

        public async Task<Bike?> GetByIdAsync(long id)
        {
            return await _set.SingleOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Bike entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
