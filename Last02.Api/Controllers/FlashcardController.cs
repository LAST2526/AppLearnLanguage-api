using Last02.Commons;
using Last02.Models.Dtos;
using Last02.Models.RequestDtos;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Last02.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashcardController : ControllerBase
    {
        private readonly IFlashcardService _flashcardService;

        public FlashcardController(IFlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
        }

        [Authorize]
        [HttpGet("by-topic-id/{topicId}")]
        public async Task<IActionResult> GetByTopicId(int topicId, [FromQuery] GetFlashcardByTopicIdRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var topics = await _flashcardService.GetByTopicIdAsync(Convert.ToInt32(userId), topicId, request);
                return topics.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<FlashcardDto>>.Error($"{nameof(GetByTopicId)} failed: {ex.Message}").ToActionResult();
            }
        }

        [Authorize]
        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateFlashcardStatusRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _flashcardService.UpdateFlashcardStatusAsync(
                Convert.ToInt32(userId),
                request.FlashcardId,
                request.Action
            );

            return updated.ToActionResult();
        }
    }
}
