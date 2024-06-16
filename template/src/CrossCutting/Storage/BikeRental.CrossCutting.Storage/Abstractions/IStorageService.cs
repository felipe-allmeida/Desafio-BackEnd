using BikeRental.CrossCutting.Storage.Models;

namespace BikeRental.CrossCutting.Storage.Abstractions
{
    public interface IStorageService
    {
        Task<string> GetBlobAsync(string container, string blobName);
        Task<BlobDto> UploadBlob(string container, string blobName, Stream dataStream, string contentType);
        Task DeleteBlob(string container, string blobName);
    }
}
