namespace BikeRental.CrossCutting.Storage.MinIO.Exceptions
{
    public class BucketFoundException : Exception
    {
        public BucketFoundException()
        { }

        public BucketFoundException(string message)
            : base(message)
        { }

        public BucketFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
