using BikeRental.Domain.Models.RentalAggregate;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Data.QueryRepositories
{
    public class RentalQueryRepository : IRentalQueryRepository
    {
        private readonly BikeRentalContext _context;
        private readonly DbSet<Rental> _set;

        public RentalQueryRepository(BikeRentalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<Rental>();
        }

        public IQueryable<Rental> GetAll()
        {
            return _set.AsQueryable().AsNoTracking();
        }
    }
}
