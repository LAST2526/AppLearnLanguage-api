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

namespace Last02.Services.Implement
{
    public class GrammarService : BaseService, IGrammarService
    {
        private readonly IUnitOfWork _uow;
        ILogger<GrammarService> _logger;
        private ILocalizedMessageService _messageService = null!;

        public GrammarService(IUnitOfWork unitOfWork, ILogger<GrammarService> logger,
            ILocalizedMessageService messageService) : base(unitOfWork)
        {
            _uow = unitOfWork;
            _logger = logger;
            _messageService = messageService;
        }

        public async Task<IEnumerable<Audio>> GetAllGrammarsAsync()
        {
            //var baseUrl = await GetBaseUrlAsync();
            var list = await _uow.Audio.GetAllAsync(a => a.AudioType == AudioType.Grammar);

            return list;
        }

        public async Task<ResponseBase<IEnumerable<GrammarDto>>> GetByCourseIdAsync(int userId, int courseId, int? pageSize, int? pageNumber)
        {
            try
            {
                var audios = await _uow.Audio.GetWithPagingAsync(
                    filter: u => u.CourseId == courseId && u.AudioType == AudioType.Grammar,
                    orderBy: u => u.OrderBy(t => t.SortOrder),
                    pageNumber: pageNumber ?? 1,
                    pageSize: pageSize ?? 10);

                if (audios == null)
                {
                    return ResponseBase<IEnumerable<GrammarDto>>.Error(
                        await _messageService.GetMessageAsync(MessageCodes.Grammar.ERR_GRAMMAR_NOT_FOUND),
                        statusCode: 404);
                }

                //var baseUrl = await GetBaseUrlAsync();

                var res = new List<GrammarDto>(audios.Count);
                foreach (var audio in audios)
                {
                    var dto = ConvertToDto(audio);
                    res.Add(dto);
                }

                return ResponseBase<IEnumerable<GrammarDto>>.Success(
                    res, await _messageService.GetMessageAsync(MessageCodes.Grammar.SUC_GRAMMAR_RETRIEVED));
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<GrammarDto>>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        public async Task<ResponseBase<GrammarDto>> GetByIdAsync(int id)
        {
            try
            {
                var audio = await _uow.Audio.GetAsync(predicate: t => t.Id == id && t.AudioType == AudioType.Grammar);

                if (audio == null)
                {
                    return ResponseBase<GrammarDto>.Error(
                        await _messageService.GetMessageAsync(MessageCodes.Grammar.ERR_GRAMMAR_NOT_FOUND),
                        statusCode: 404);
                }

                //var baseUrl = await GetBaseUrlAsync();
                var dto = ConvertToDto(audio);

                return ResponseBase<GrammarDto>.Success(dto, "");
            }
            catch (Exception ex)
            {
                return ResponseBase<GrammarDto>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        public async Task<ResponseBase<GrammarDto>> GetByGrammarCodeAsync(string code)
        {
            try
            {
                var audio = await _uow.Audio.GetAsync(predicate: t => t.AudioCode == code && t.AudioType == AudioType.Grammar);

                if (audio == null)
                {
                    return ResponseBase<GrammarDto>.Error(
                        await _messageService.GetMessageAsync(MessageCodes.Grammar.ERR_GRAMMAR_NOT_FOUND),
                        statusCode: 404);
                }

                //var baseUrl = await GetBaseUrlAsync();
                var dto = ConvertToDto(audio);

                return ResponseBase<GrammarDto>.Success(dto, "");
            }
            catch (Exception ex)
            {
                return ResponseBase<GrammarDto>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        //private async Task<string> GetBaseUrlAsync(CancellationToken ct = default)
        //    => (await _appSettingService.GetAudioBaseUrlAsync(ct)).TrimEnd('/');

        public static GrammarDto ConvertToDto(Audio audio)
            => new GrammarDto
            {
                Id = audio.Id,
                AudioCode = audio.AudioCode,
                AudioType = audio.AudioType ?? AudioType.Grammar,
                Title = audio.Title,
                FileUrl = audio.FileUrl,
                SortOrder = audio.SortOrder,
                IsFree = audio.IsFree,
                Script = audio.Script,
                ScriptEn = audio.ScriptEn,
                ScriptVi = audio.ScriptVi,
                CourseId = audio.CourseId,
            };
    }
}
