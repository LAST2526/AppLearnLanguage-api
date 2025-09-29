using Amazon.S3.Model;
using Last02.Commons;
using Last02.Data.UnitOfWork;
using Last02.Models.Dtos;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Last02.Services.Implement
{
    public class AudioService : BaseService, IAudioService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<IAudioService> _logger;
        private ILocalizedMessageService _messageService = null!;
        private IStorageService _storageService = null!;

        public AudioService(IUnitOfWork unitOfWork, ILogger<IAudioService> logger,
            ILocalizedMessageService messageService, IStorageService storageService) : base(unitOfWork)
        {
            _uow = unitOfWork;
            _logger = logger;
            _storageService = storageService;
            _messageService = messageService;
        }

        public async Task<IEnumerable<Audio>> GetAllAsync()
        {
            var list = await _uow.Audio.GetAllAsync();

            return list;
        }

        public async Task<IEnumerable<Audio>> GetByCourseIdAsync(int courseId)
        {
            var list = await _uow.Audio.GetQueryable()
                .Where(a => a.CourseId == courseId)
                .ToListAsync();

            return list;
        }

        public Task<(Stream Stream, string ContentType, long? Length, string? ETag, DateTimeOffset? LastModified, TimeSpan? Duration)?> GetAudioAsync(string encodedBlobName)
        {
            throw new NotImplementedException();
        }

        //public async Task<ResponseBase<QrCodeScanResponseDto>> GetAudioPageByQrCode(int courseId, string code, int? pageSize)
        //{
        //    int actualPageSize = pageSize > 0 ? pageSize.Value : 10;

        //    var currentAudio = await _uow.Audio.GetAsync(a => a.AudioCode == code && a.CourseId == courseId);
        //    if (currentAudio == null)
        //    {
        //        return ResponseBase<QrCodeScanResponseDto>.Error(
        //            await _messageService.GetMessageAsync(MsgCodes.QrCode.ERR_QRCODE_NOT_FOUND),
        //            statusCode: 404);
        //    }

        //    var baseUrl = await GetBaseUrlAsync();

        //    var audioType = currentAudio.AudioType ?? AudioType.Kaiwa;

        //    bool hasValidSortOrder = await _uow.Audio
        //        .GetQueryable(a => a.CourseId == courseId && a.AudioType == audioType)
        //        .AnyAsync(a => a.SortOrder != 0);

        //    int currentIndex;
        //    int currentPage;
        //    List<Audio> pageAudios;

        //    if (hasValidSortOrder)
        //    {
        //        currentIndex = await _uow.Audio.CountAsync(a =>
        //            a.AudioType == audioType &&
        //            a.CourseId == courseId &&
        //            a.SortOrder < currentAudio.SortOrder);

        //        currentPage = (currentIndex / actualPageSize) + 1;

        //        pageAudios = await _uow.Audio.GetWithPagingAsync(
        //            filter: a => a.AudioType == audioType && a.CourseId == courseId,
        //            orderBy: q => q.OrderBy(a => a.SortOrder),
        //            pageNumber: currentPage,
        //            pageSize: actualPageSize
        //        );
        //    }
        //    else
        //    {
        //        currentIndex = await _uow.Audio.CountAsync(a =>
        //            a.AudioType == audioType &&
        //            a.CourseId == courseId &&
        //            a.Id < currentAudio.Id);

        //        currentPage = (currentIndex / actualPageSize) + 1;

        //        pageAudios = await _uow.Audio.GetWithPagingAsync(
        //            filter: a => a.AudioType == audioType && a.CourseId == courseId,
        //            orderBy: q => q.OrderBy(a => a.Id),
        //            pageNumber: currentPage,
        //            pageSize: actualPageSize
        //        );
        //    }

        //    var responseDto = new QrCodeScanResponseDto
        //    {
        //        CurrentAudio = MapToDto(currentAudio, baseUrl),
        //        ListAudios = pageAudios.Select(a => MapToDto(a, baseUrl)).ToList(),
        //        CurrentPage = currentPage,
        //        PageSize = actualPageSize,
        //    };

        //    return ResponseBase<QrCodeScanResponseDto>.Success(
        //        responseDto,
        //        await _messageService.GetMessageAsync(MsgCodes.QrCode.SUC_QRCODE_RETRIEVED));
        //}

        //public async Task<(Stream Stream, string ContentType, long? Length, string? ETag, DateTimeOffset? LastModified, TimeSpan? Duration)?> GetAudioAsync(string encodedBlobName)
        //{
        //    if (string.IsNullOrWhiteSpace(encodedBlobName))
        //        return null;

        //    try
        //    {
        //        var decodedName = Uri.UnescapeDataString(encodedBlobName);
        //        var blobClient = _storageService.GetBlobClient(decodedName);

        //        if (!await blobClient.ExistsAsync())
        //            return null;

        //        var props = await blobClient.GetPropertiesAsync();

        //        // Content-Type
        //        var contentType = string.IsNullOrWhiteSpace(props.Value.ContentType)
        //            ? "application/octet-stream"
        //            : props.Value.ContentType;

        //        // Mở stream (seekable)
        //        var stream = await blobClient.OpenReadAsync();

        //        // Lấy duration:
        //        // 1) Ưu tiên metadata "durationSeconds" nếu có
        //        TimeSpan? duration = null;
        //        if (props.Value.Metadata != null &&
        //            props.Value.Metadata.TryGetValue("durationSeconds", out var durStr) &&
        //            int.TryParse(durStr, out var durSec))
        //        {
        //            duration = TimeSpan.FromSeconds(durSec);
        //        }

        //        // 2) Nếu metadata không có, dùng TagLib để đọc từ stream rồi reset Position
        //        if (duration is null)
        //        {
        //            try
        //            {
        //                if (stream.CanSeek)
        //                {
        //                    var abs = new StreamAbstraction(decodedName, stream);
        //                    using var tagFile = TagLib.File.Create(abs);
        //                    duration = tagFile.Properties?.Duration;
        //                    stream.Position = 0; // RẤT quan trọng
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogWarning(ex, "Không đọc được duration từ audio: {Name}", decodedName);
        //            }
        //        }

        //        return (stream,
        //                contentType,
        //                props.Value.ContentLength,
        //                props.Value.ETag.ToString(),
        //                props.Value.LastModified,
        //                duration);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error reading blob audio: {Name}", encodedBlobName);
        //        return null;
        //    }
        //}

        //private async Task<string> GetBaseUrlAsync(CancellationToken ct = default)
        //{
        //    return (await _appSettingService.GetAudioBaseUrlAsync(ct)).TrimEnd('/');
        //}

        private AudioDto MapToDto(Audio a, string baseUrl) => new AudioDto
        {
            Id = a.Id,
            AudioCode = a.AudioCode,
            Title = a.Title,
            AudioType = a.AudioType ?? AudioType.Kaiwa,
            FileUrl = a.FileUrl,
            SortOrder = a.SortOrder,
            IsFree = a.IsFree,
            Script = a.Script,
            ScriptVi = a.ScriptVi,
            ScriptEn = a.ScriptEn,
            CourseId = a.CourseId,
            //QrCode = a.QrCode
        };

        // ---- Helper: tránh TagLib tự Dispose stream gốc ----
        //private class StreamAbstraction : TagLib.File.IFileAbstraction
        //{
        //    public StreamAbstraction(string name, Stream s) { Name = name; ReadStream = s; WriteStream = s; }
        //    public string Name { get; }
        //    public Stream ReadStream { get; }
        //    public Stream WriteStream { get; }
        //    public void CloseStream(Stream stream) { /* no-op */ }
        //}
    }
}
