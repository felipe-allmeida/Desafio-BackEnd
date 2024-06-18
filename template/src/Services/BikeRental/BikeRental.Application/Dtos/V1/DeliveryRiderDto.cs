namespace BikeRental.Application.DTOs.V1
{
    public record DeliveryRiderDto
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public string Cnpj { get; init; }
        public CnhDto? Cnh { get; init; }
        public bool IsDeleted { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
    }
}
