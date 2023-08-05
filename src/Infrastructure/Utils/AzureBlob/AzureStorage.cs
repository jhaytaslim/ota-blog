using Azure.Storage.Blobs;
using Domain.ConfigurationModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Utils.Storage;

public class AzureStorage : IAzureStorage
{
    private readonly AzureConfigurations _azureConfigurations;
    private readonly IConfiguration _configuration;

    public AzureStorage(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _azureConfigurations = new AzureConfigurations();
        _configuration.Bind(_azureConfigurations.Section, _azureConfigurations);
    }

    public async Task<string> UploadAsync(IFormFile blob)
    {

        // Get a reference to a container named in appsettings.json and then create it
        BlobContainerClient container = new BlobContainerClient(_azureConfigurations.BlobConnectionString, _azureConfigurations.BlobContainerName);

        // Get a reference to the blob just uploaded from the API in a container from configuration settings
        BlobClient client = container.GetBlobClient(String.Concat(DateTimeOffset.Now.ToUnixTimeMilliseconds(), blob.FileName));
        // Open a stream for the file we want to upload
        await using (Stream? data = blob.OpenReadStream())
        {
            // Upload the file async
            await client.UploadAsync(data);
        }

        return client.Uri.AbsoluteUri;
    }
    public async Task<bool> DeleteAsync(string blobFilename)
    {
        BlobContainerClient client = new BlobContainerClient(_azureConfigurations.BlobConnectionString, _azureConfigurations.BlobContainerName);

        BlobClient file = client.GetBlobClient(blobFilename);

        // Delete the file
        var response = await file.DeleteAsync();

        return response.IsError ? false : true;
    }

    public async Task<Stream> DownloadAsync(string blobFilename)
    {
        // Get a reference to a container named in appsettings.json and then create it
        BlobContainerClient container = new BlobContainerClient(_azureConfigurations.BlobConnectionString, _azureConfigurations.BlobContainerName);

        BlobClient client = container.GetBlobClient(blobFilename);

        var downloadContent = await client.DownloadAsync();

        return downloadContent.Value.Content;
    }

}
