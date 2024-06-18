namespace BikeRental.CrossCutting.MinIO.Options
{
    public record MinIOOptions
    {
        public string ExternalUrl { get; init; }
        public string Endpoint { get; init; }
        public int Port { get; init; }
        public string AccessKey { get; init; }
        public string SecretKey { get; init; }
    }
}
