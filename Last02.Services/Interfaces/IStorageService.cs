using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder, bool useCustomName = false);
        string GenerateDownloadUrl(string blobName, int validMinutes = 60);
        string GetContainerName();
    }
}
