using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Last02.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Implement
{
    public class S3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3;
        private readonly string _bucket;
        private readonly ILogger<S3StorageService> _logger;

        public S3StorageService(IAmazonS3 s3, IConfiguration config, ILogger<S3StorageService> logger)
        {
            _s3 = s3;
            _bucket = config["AWS:Bucket"] ?? throw new ArgumentNullException("AWS:Bucket");
            _logger = logger;
        }

        public string GetContainerName() => _bucket;

        public string GenerateDownloadUrl(string blobName, int expiryMinutes = 0)
        {
            if (string.IsNullOrWhiteSpace(blobName))
                return string.Empty;

            var safeKey = string.Join("/",
                blobName.Split('/', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Uri.EscapeDataString));

            return $"https://{_bucket}.s3.amazonaws.com/{safeKey}";
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder, bool useCustomName = false)
        {
            if (fileStream == null) throw new ArgumentNullException(nameof(fileStream));
            if (fileStream.CanSeek) fileStream.Position = 0;

            var ext = Path.GetExtension(fileName);
            var finalName = useCustomName ? fileName : $"{Guid.NewGuid()}{ext}";

            folder = (folder ?? "").Trim().Trim('/');
            var key = string.IsNullOrWhiteSpace(folder) ? finalName : $"{folder}/{finalName}";
            key = key.Replace("\\", "/").TrimStart('/');

            try
            {
                var transfer = new TransferUtility(_s3);

                var req = new TransferUtilityUploadRequest
                {
                    BucketName = _bucket,
                    Key = key,
                    InputStream = fileStream,
                    ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType,
                    AutoCloseStream = false
                };

                await transfer.UploadAsync(req);
                return key;
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex,
                    "S3 Upload FAILED. Bucket={Bucket}, Key={Key}, Status={Status}, ErrorCode={ErrorCode}, RequestId={RequestId}, Message={Message}",
                    _bucket, key, ex.StatusCode, ex.ErrorCode, ex.RequestId, ex.Message);

                throw;
            }
        }

    }
}
