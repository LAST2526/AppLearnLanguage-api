using Amazon.S3.Model;
using Last02.Commons;
using Last02.Data.Entities;
using Last02.Data.UnitOfWork;
using Last02.Models;
using Last02.Models.Dtos;
using Last02.Models.RequestDtos;
using Last02.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Implement
{
    internal class FlashcardService : BaseService, IFlashcardService
    {
        private readonly IUnitOfWork _uow;
        ILogger<FlashcardService> _logger;
        private IMemberService _memberService = null!;
        private ILocalizedMessageService _messageService = null!;

        public FlashcardService(IUnitOfWork unitOfWork, ILogger<FlashcardService> logger, IOptions<GoogleAuthSettings> googleAuthSettings
            , IOptions<JwtSettings> jwtSettings, IPasswordService passwordService
            , IMemberService memberService, ILocalizedMessageService messageService) : base(unitOfWork)
        {
            _uow = unitOfWork;
            _logger = logger;
            _memberService = memberService;
            _messageService = messageService;
        }

        public async Task<ResponseBase<IEnumerable<FlashcardDto>>> GetByTopicIdAsync(int userId, int topicId, GetFlashcardByTopicIdRequest request)
        {
            try
            {

                var member = _memberService.GetByUserId(userId);
                if (member == null)
                {
                    return ResponseBase<IEnumerable<FlashcardDto>>.Error(await _messageService.GetMessageAsync(MessageCodes.Flashcard.ERR_FLASHCARD_MEMBER_NOT_FOUND));
                }
                await ResetStatusFlashcardsAsync(member.Id);

                var allFlashcards = await _uow.Flashcard.GetAllAsync(
                            filter: u => u.TopicId == topicId,
                            orderBy: u => u.OrderBy(t => t.Id));

                if (allFlashcards == null || !allFlashcards.Any())
                {
                    return ResponseBase<IEnumerable<FlashcardDto>>.Error(await _messageService.GetMessageAsync(MessageCodes.Flashcard.ERR_FLASHCARD_NOT_FOUND), statusCode: 404);
                }

                var flashcardIds = allFlashcards.Select(f => f.Id).ToList();

                var memberFlashcardStatuses = await _uow.MemberFlashcard.GetAllAsync(
                    filter: mf => mf.MemberId == member.Id && flashcardIds.Contains(mf.FlashcardId));

                var statusDict = memberFlashcardStatuses.ToDictionary(mf => mf.FlashcardId, mf => mf.Status);

                var filtered = allFlashcards
                    .Select(f =>
                    {
                        var dto = ConvertToDto(f);
                        dto.Status = statusDict.TryGetValue(f.Id, out var status) ? status : FlashcardStatus.New;
                        return dto;
                    })
                    .Where(dto =>
                        request.Status == null ||
                        dto.Status.ToString() == request.Status.ToString())
                    .ToList();

                if (request.PageNumber != null && request.PageSize != null)
                {
                    var paged = filtered
                        .Skip(((request.PageNumber ?? 1) - 1) * (request.PageSize ?? 10))
                        .Take(request.PageSize ?? 10)
                        .ToList();
                    return ResponseBase<IEnumerable<FlashcardDto>>.Success(paged, await _messageService.GetMessageAsync(MessageCodes.Flashcard.SUC_FLASHCARD_RETRIEVED));
                }

                return ResponseBase<IEnumerable<FlashcardDto>>.Success(filtered, await _messageService.GetMessageAsync(MessageCodes.Flashcard.SUC_FLASHCARD_RETRIEVED));
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<FlashcardDto>>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        public static FlashcardDto ConvertToDto(Flashcard flashcard)
        {
            return new FlashcardDto
            {
                Id = flashcard.Id,
                FlashcardCode = flashcard.FlashcardCode,
                TopicId = flashcard.TopicId,
                Front = flashcard.Front,
                MeaningVi = flashcard.MeaningVi ?? string.Empty,
                MeaningEn = flashcard.MeaningEn ?? string.Empty,
                Furigana = flashcard.Furigana ?? string.Empty,
                Example = flashcard.Example,
                ExampleVi = flashcard.ExampleVi,
                ExampleEn = flashcard.ExampleEn,
                ImageUrl = flashcard.ImageUrl,
            };
        }

        public FlashcardStatus GetFlashcardStatusByRememberCount(int rememberCount)
        {
            if (rememberCount == 5)
            {
                return FlashcardStatus.Completed;
            }
            else if (rememberCount == 0)
            {
                return FlashcardStatus.New;
            }
            else
            {
                return FlashcardStatus.InProgress;
            }
        }

        public async Task ResetStatusFlashcardsAsync(int memberId)
        {
            var now = DateTime.UtcNow;

            var expiredFlashcards = await _uow.MemberFlashcard.GetQueryable()
                .Where(mf => mf.MemberId == memberId && mf.Status == FlashcardStatus.InProgress && mf.RememberCount > 0 && mf.NextReviewAt <= now)
                .ToListAsync();

            foreach (var flashcard in expiredFlashcards)
            {
                flashcard.Status = FlashcardStatus.New;
                _uow.MemberFlashcard.Update(flashcard);
            }

            await _uow.SaveAsync();
        }

        public DateTime? GetNextReviewDate(int rememberCount)
        {
            return rememberCount switch
            {
                0 => null,
                1 => DateTime.UtcNow.AddMinutes(3),
                2 => DateTime.UtcNow.AddMinutes(5),
                3 => DateTime.UtcNow.AddMinutes(10),
                4 => DateTime.UtcNow.AddMinutes(20),
                _ => DateTime.UtcNow
            };
            //return rememberCount switch
            //{
            //    0 => null,
            //    1 => DateTime.UtcNow.AddDays(1),
            //    2 => DateTime.UtcNow.AddDays(3),
            //    3 => DateTime.UtcNow.AddDays(7),
            //    4 => DateTime.UtcNow.AddDays(30),
            //    _ => DateTime.UtcNow
            //};
        }

        public async Task<ResponseBase<MemberFlashcard>> UpdateFlashcardStatusAsync(int userId, int flashcardId, FlashcardAction status)
        {
            try
            {
                var member = _memberService.GetByUserId(userId);
                if (member == null)
                {
                    return ResponseBase<MemberFlashcard>.Error(await _messageService.GetMessageAsync(MessageCodes.Flashcard.ERR_FLASHCARD_MEMBER_NOT_FOUND));
                }

                var entry = await _uow.MemberFlashcard.GetQueryable()
                    .FirstOrDefaultAsync(mf => mf.MemberId == member.Id && mf.FlashcardId == flashcardId);

                if (entry == null)
                {
                    entry = new MemberFlashcard
                    {
                        MemberId = member.Id,
                        FlashcardId = flashcardId,
                        Status = FlashcardStatus.New,
                        LastReviewedAt = DateTime.UtcNow,
                        RememberCount = 0,
                    };
                    await _uow.MemberFlashcard.AddAsync(entry);
                }
                else
                {
                    entry.LastReviewedAt = DateTime.UtcNow;
                    _uow.MemberFlashcard.Update(entry);
                }

                switch (status)
                {
                    case FlashcardAction.Forgot:
                        entry.RememberCount = 0;
                        entry.Status = GetFlashcardStatusByRememberCount(entry.RememberCount);
                        entry.NextReviewAt = GetNextReviewDate(entry.RememberCount);
                        break;
                    case FlashcardAction.Remembered:
                        entry.RememberCount = Math.Min(entry.RememberCount + 1, 5);
                        entry.Status = GetFlashcardStatusByRememberCount(entry.RememberCount);
                        entry.NextReviewAt = GetNextReviewDate(entry.RememberCount);
                        break;
                    default:
                        break;
                }

                await _uow.SaveAsync();
                return ResponseBase<MemberFlashcard>.Success(entry);
            }
            catch (Exception ex)
            {
                return ResponseBase<MemberFlashcard>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        public async Task<int> CountByTopicIdAsync(int userId, int topicId, GetFlashcardByTopicIdRequest request)
        {
            try
            {

                var member = _memberService.GetByUserId(userId);
                if (member == null)
                {
                    return 0;
                }
                await ResetStatusFlashcardsAsync(member.Id);

                var allFlashcards = await _uow.Flashcard.GetAllAsync(
                    filter: u => u.TopicId == topicId,
                    orderBy: u => u.OrderBy(t => t.Id));

                if (allFlashcards == null || !allFlashcards.Any())
                {
                    return 0;
                }

                var flashcardIds = allFlashcards.Select(f => f.Id).ToList();

                var memberFlashcardStatuses = await _uow.MemberFlashcard.GetAllAsync(
                    filter: mf => mf.MemberId == member.Id && flashcardIds.Contains(mf.FlashcardId));

                var statusDict = memberFlashcardStatuses.ToDictionary(mf => mf.FlashcardId, mf => mf.Status);

                var count = allFlashcards
                    .Select(f =>
                    {
                        var dto = ConvertToDto(f);
                        dto.Status = statusDict.TryGetValue(f.Id, out var status) ? status : FlashcardStatus.New;
                        return dto;
                    })
                    .Count(dto =>
                        request.Status == null ||
                        dto.Status.ToString() == request.Status.ToString());

                return count;
            }
            catch
            {
                return 0;
            }
        }
    }
}
