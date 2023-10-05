using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;

namespace Batch_Manager.Services
{
    public class BlobStorageClient : IBlobStorageClient
    {
        public BlobContainerClient GetBlobContainerClient(IConfiguration configuration, string blobContainerName)
        {
            var kvUrl = configuration.GetValue<string>("AzureKeyVaultUrl");
            var secretClient = new SecretClient(new Uri(kvUrl.ToString()), new DefaultAzureCredential());
            string storageAccountConnectionString = (secretClient.GetSecret("StorageAccountConnectionString").Value).Value;
            return new BlobContainerClient(storageAccountConnectionString, blobContainerName);
        }
    }
}
