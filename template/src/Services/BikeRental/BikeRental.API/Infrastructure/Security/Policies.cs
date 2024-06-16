namespace BikeRental.API.Infrastructure.Security
{
    public static class Policies
    {
        public const string AllowSpecificOrigins = "allow_specific_origins";
        public const string NotAnonymous = "not_anonymous";

        public const string AdminRead = "admin_read";
        public const string AdminWrite = "admin_write";

        public const string DeliveryRiderRead = "deliveryrider_read";
        public const string DeliveryRiderWrite = "deliveryrider_write";
    }
}
