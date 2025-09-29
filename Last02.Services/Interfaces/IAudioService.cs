using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;

namespace Last02.Services.Interfaces
{
    public interface IAudioService
    {
        Task<IEnumerable<Audio>> GetAllAsync();
        Task<IEnumerable<Audio>> GetByCourseIdAsync(int courseId);
        //Task<ResponseBase<QrCodeScanResponseDto>> GetAudioPageByQrCode(int courseId, string code, int? pageSize);
        Task<(Stream Stream, string ContentType, long? Length, string? ETag, DateTimeOffset? LastModified, TimeSpan? Duration)?> GetAudioAsync(string encodedBlobName);
    }
}
