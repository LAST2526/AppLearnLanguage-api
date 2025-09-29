using Last02.Commons;
using Last02.Data.UnitOfWork;
using Last02.Models.Dtos;
using Last02.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;
using Last02.Models.RequestDtos;

namespace Last02.Services.Implement
{
    public class TopicService : BaseService, ITopicService
    {
        private readonly IUnitOfWork _uow;
        ILogger<TopicService> _logger;
        private ILocalizedMessageService _messageService = null!;
        private IFlashcardService _flashcardService = null!;

        public TopicService(IUnitOfWork unitOfWork, ILogger<TopicService> logger
            , IFlashcardService flashcardService, ILocalizedMessageService messageService) : base(unitOfWork)
        {
            _uow = unitOfWork;
            _logger = logger;
            _messageService = messageService;
            _flashcardService = flashcardService;
        }

        public async Task<ResponseBase<IEnumerable<TopicListItemDto>>> GetByCourseIdAsync(int userId, int courseId, int? pageSize, int? pageNumber)
        {
            try
            {
                var topics = new List<Topic>();

                if (pageNumber != null && pageSize != null)
                {
                    topics = await _uow.Topic.GetWithPagingAsync(
                        filter: u => u.CourseId == courseId,
                        orderBy: u => u.OrderBy(t => t.Id),
                        pageNumber: pageNumber ?? 1,
                        pageSize: pageSize ?? 10);
                }
                else
                {
                    topics = await _uow.Topic.GetAllAsync(
                        filter: u => u.CourseId == courseId,
                        orderBy: u => u.OrderBy(t => t.Id));
                }

                if (topics == null)
                {
                    return ResponseBase<IEnumerable<TopicListItemDto>>.Error(await _messageService.GetMessageAsync(MessageCodes.Topic.ERR_TOPIC_NOT_FOUND), statusCode: 404);
                }

                var res = new List<TopicListItemDto>();
                foreach (var topic in topics)
                {
                    var dto = ConvertToListItemDto(topic);
                    dto.FlashcardCnt = await _flashcardService.CountByTopicIdAsync(userId, topic.Id, new GetFlashcardByTopicIdRequest());
                    dto.CompletedFlashcardCnt = await _flashcardService.CountByTopicIdAsync(userId, topic.Id, new GetFlashcardByTopicIdRequest() { Status = FlashcardStatus.Completed });

                    res.Add(dto);
                }

                return ResponseBase<IEnumerable<TopicListItemDto>>.Success(res, await _messageService.GetMessageAsync(MessageCodes.Topic.SUC_TOPIC_RETRIEVED));
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<TopicListItemDto>>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        public static TopicListItemDto ConvertToListItemDto(Topic topic)
        {
            return new TopicListItemDto
            {
                Id = topic.Id,
                Title = topic.Title,
                TopicCode = topic.TopicCode,
                Description = topic.Description,
                IsFree = topic.IsFree,
                HexColorCode = topic.HexColorCode,
                CourseId = topic.CourseId
            };
        }
    }
}
