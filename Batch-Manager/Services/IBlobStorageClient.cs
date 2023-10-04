using Azure.Storage.Blobs;

namespace Batch_Manager.Services
{
    public interface IBlobStorageClient
    {
        BlobContainerClient GetBlobContainerClient(IConfiguration configuration, string blobContainerName);
    }
}
