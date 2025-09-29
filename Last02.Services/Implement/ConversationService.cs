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
    public class ConversationService : BaseService, IConversationService
    {
        private readonly IUnitOfWork _uow;
        ILogger<ConversationService> _logger;
        private ILocalizedMessageService _messageService = null!;

        public ConversationService(IUnitOfWork unitOfWork, ILogger<ConversationService> logger
            , ILocalizedMessageService messageService) : base(unitOfWork)
        {
            _uow = unitOfWork;
            _logger = logger;
            _messageService = messageService;
        }

        public async Task<IEnumerable<Audio>> GetAllConversationsAsync()
        {
            //var baseUrl = await GetBaseUrlAsync();
            var list = await _uow.Audio.GetAllAsync(a => a.AudioType == AudioType.Kaiwa);

            //foreach (var a in list)
            //    a.AbsoluteFileUrl = UrlHelper.ToAbsoluteUrl(a.FileUrl, baseUrl);

            return list;
        }

        public async Task<ResponseBase<IEnumerable<ConversationDto>>> GetByCourseIdAsync(int userId, int courseId, int? pageSize, int? pageNumber)
        {
            try
            {
                var conversations = await _uow.Audio.GetWithPagingAsync(
                        filter: u => u.CourseId == courseId && u.AudioType == AudioType.Kaiwa,
                        orderBy: u => u.OrderBy(t => t.Id),
                        pageNumber: pageNumber ?? 1,
                        pageSize: pageSize ?? 10);

                if (conversations == null)
                {
                    return ResponseBase<IEnumerable<ConversationDto>>.Error(
                        await _messageService.GetMessageAsync(MessageCodes.Conversation.ERR_CONVERSATION_NOT_FOUND),
                        statusCode: 404);
                }

                //var baseUrl = await GetBaseUrlAsync();

                var res = new List<ConversationDto>(conversations.Count);
                foreach (var conversation in conversations)
                {
                    var dto = ConvertToDto(conversation);
                    res.Add(dto);
                }

                return ResponseBase<IEnumerable<ConversationDto>>.Success(
                    res, await _messageService.GetMessageAsync(MessageCodes.Conversation.SUC_CONVERSATION_RETRIEVED));
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<ConversationDto>>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        public async Task<ResponseBase<ConversationDto>> GetByIdAsync(int id)
        {
            try
            {
                var conversation = await _uow.Audio.GetAsync(predicate: t => t.Id == id && t.AudioType == AudioType.Kaiwa);

                if (conversation == null)
                {
                    return ResponseBase<ConversationDto>.Error(
                        await _messageService.GetMessageAsync(MessageCodes.Conversation.ERR_CONVERSATION_NOT_FOUND),
                        statusCode: 404);
                }

                //var baseUrl = await GetBaseUrlAsync();
                var dto = ConvertToDto(conversation);

                return ResponseBase<ConversationDto>.Success(dto, "");
            }
            catch (Exception ex)
            {
                return ResponseBase<ConversationDto>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        public async Task<ResponseBase<ConversationDto>> GetByConversationCodeAsync(string code)
        {
            try
            {
                var conversation = await _uow.Audio.GetAsync(predicate: t => t.AudioCode == code && t.AudioType == AudioType.Kaiwa);

                if (conversation == null)
                {
                    return ResponseBase<ConversationDto>.Error(
                        await _messageService.GetMessageAsync(MessageCodes.Conversation.ERR_CONVERSATION_NOT_FOUND),
                        statusCode: 404);
                }

                //var baseUrl = await GetBaseUrlAsync();
                var dto = ConvertToDto(conversation);

                return ResponseBase<ConversationDto>.Success(dto, "");
            }
            catch (Exception ex)
            {
                return ResponseBase<ConversationDto>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        //private async Task<string> GetBaseUrlAsync(CancellationToken ct = default)
        //{
        //    return (await _appSettingService.GetAudioBaseUrlAsync(ct)).TrimEnd('/');
        //}

        public static ConversationDto ConvertToDto(Audio audio)
        {
            return new ConversationDto
            {
                Id = audio.Id,
                AudioCode = audio.AudioCode,
                AudioType = audio.AudioType ?? AudioType.Kaiwa,
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
}
