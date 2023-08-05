using Microsoft.AspNetCore.Http;

namespace Infrastructure.Data.Utils.Storage;

public interface IAzureStorage
{
    Task<string> UploadAsync(IFormFile file);
    Task<bool> DeleteAsync(string blobFilename);
    Task<Stream> DownloadAsync(string blobFilename);
}

