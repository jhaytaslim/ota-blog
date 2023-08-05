

using System.IO;
using Microsoft.AspNetCore.Http;

namespace Application.Helpers;

public static class FileHelper
{
    public static IFormFile ConvertToIFormFile(this string image, string name)
    {
        byte[] bytes = Convert.FromBase64String(image);
        MemoryStream stream = new MemoryStream(bytes);

        IFormFile file = new FormFile(stream, 0, bytes.Length, name, name);
        return file;
    }
}

