using BikeRental.Domain.Models.BikeAggregate;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.QueryRepositories
{
    public class BikeQueryRepository : IBikeQueryRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<Bike> _bikeSet;

        public BikeQueryRepository(BikeRentalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _bikeSet = context.Set<Bike>();
        }

        public IQueryable<Bike> GetAll()
        {
            return _bikeSet.AsQueryable().AsNoTracking();
        }
    }
}
