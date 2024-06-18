using BikeRental.CrossCutting.MinIO.Options;
using BikeRental.CrossCutting.Storage.Abstractions;
using BikeRental.CrossCutting.Storage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.Text.Json;
using System.Xml.Linq;

namespace BikeRental.CrossCutting.Storage.MinIO
{
    public class MinIOService : IStorageService
    {

        private readonly IMinioClient _client;
        private readonly MinIOOptions _options;

        public MinIOService(IMinioClient client, IOptions<MinIOOptions> options)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options.Value;
        }

        public async Task<string> GetBlobAsync(string container, string blobName)
        {
            if (!await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(container)))
            {
                throw new BucketNotFoundException($"Bucket '{container}' not found");
            }

            var response = await _client.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(container)

                .WithObject(blobName)
                .WithExpiry(3600)
            );

            return response;
        }

        public async Task<BlobDto> UploadBlob(string container, string blob, Stream stream, string contentType)
        {
            if (!await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(container)))
            {
                await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(container));

                string policy = $@"{{
    ""Version"": ""2012-10-17"",
    ""Statement"": [
        {{
            ""Effect"": ""Allow"",
            ""Principal"": {{
                ""AWS"": [""*""]
            }},
            ""Action"": [
                ""s3:ListBucketMultipartUploads"",
                ""s3:GetBucketLocation"",
                ""s3:ListBucket""
            ],
            ""Resource"": [""arn:aws:s3:::{container}""]
        }},
        {{
            ""Effect"": ""Allow"",
            ""Principal"": {{
                ""AWS"": [""*""]
            }},
            ""Action"": [
                ""s3:PutObject"",
                ""s3:AbortMultipartUpload"",
                ""s3:DeleteObject"",
                ""s3:GetObject"",
                ""s3:ListMultipartUploadParts""
            ],
            ""Resource"": [""arn:aws:s3:::{container}/*""]
        }}
    ]
}};";
                await _client.SetPolicyAsync(new SetPolicyArgs().WithBucket(container).WithPolicy(policy));
            }

            var response = await _client.PutObjectAsync(new PutObjectArgs()
                .WithBucket(container)
                .WithObject(blob)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithHeaders(new Dictionary<string, string>
                {
                    { "Content-Type", contentType },
                    { "Cache-Control", "public, max-age=0" }
                })
                .WithContentType(contentType));

            //http://localhost:9000/test/documents/cnh.jpeg
            //http(s)://<minio-server-endpoint>:<port>/<bucket-name>/<object-key>
            //http://localhost:9000/test/documents/cnh.jpeg


            return new BlobDto
            {
                ETag = response.Etag,
                BlobName = response.ObjectName,
                Url = $"{_options.ExternalUrl}/{container}/{response.ObjectName}"
            };
        }

        public Task DeleteBlob(string container, string blobName)
        {
            throw new NotImplementedException();
        }
    }

}
