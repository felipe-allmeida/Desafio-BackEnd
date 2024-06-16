using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.CrossCutting.Storage.Models
{
    public record BlobDto
    {
        public string ETag { get; init; }
        public string BlobName { get; init; }
    }
}
