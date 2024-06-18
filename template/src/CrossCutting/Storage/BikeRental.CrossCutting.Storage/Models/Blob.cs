using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.CrossCutting.Storage.Models
{
    public record BlobDto
    {
        public required string ETag { get; init; }
        public required string BlobName { get; init; }
        public required string Url { get; init; }
    }
}
