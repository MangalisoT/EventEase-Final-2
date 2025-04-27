using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace EventEase_Final.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

            var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");

            using (var stream = file.OpenReadStream())
            {
                stream.Position = 0;
                await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
            }

            return blobClient.Uri.ToString();
        }
    }
}
