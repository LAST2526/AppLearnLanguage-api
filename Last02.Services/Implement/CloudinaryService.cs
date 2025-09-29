using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Implement
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string? _folder;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:Cloud"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];
            _folder = configuration["Cloudinary:Folder"];

            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = _folder
            };
            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };
            return await _cloudinary.DestroyAsync(deletionParams);
        }

        /// <summary>
        /// Extract Cloudinary public_id từ URL
        /// </summary>
        public string GetPublicIdFromUrl(string imageUrl)
        {
            // Ví dụ URL: https://res.cloudinary.com/demo/image/upload/v1692459123/folder/avatar_abc123.jpg
            var uri = new Uri(imageUrl);
            var segments = uri.AbsolutePath.Split('/');
            // lấy từ sau "upload/"
            var index = Array.IndexOf(segments, "upload");
            var publicId = string.Join("/", segments.Skip(index + 2)); // bỏ qua upload/v...
            return Path.Combine(Path.GetDirectoryName(publicId) ?? "", Path.GetFileNameWithoutExtension(publicId))
                .Replace("\\", "/");
        }
    }
}
