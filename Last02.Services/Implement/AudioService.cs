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
    }
}
