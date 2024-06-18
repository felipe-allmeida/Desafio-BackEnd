using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.DeliveryRiderAggregate
{
    public interface IDeliveryRiderRepository : IRepository<DeliveryRider>
    {
        Task<DeliveryRider?> GetByIdAsync(long id);
        Task<DeliveryRider?> GetByUserIdAsync(string userId);
        Task<bool> ExistsByCnpjAsync(string cnpj);
        Task<bool> ExistsByCnhAsync(string cnh);
    }
}
