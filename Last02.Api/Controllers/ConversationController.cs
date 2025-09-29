using Last02.Commons;
using Last02.Models.Dtos;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Last02.Data.Entities;

namespace Last02.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        /// <summary>
        /// Gets all conversations
        /// </summary>
        /// <returns>List of all conversations</returns>
        /// <response code="200">Returns the list of conversations</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseBase<IEnumerable<Audio>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseBase<IEnumerable<Audio>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var conversations = await _conversationService.GetAllConversationsAsync();
                return ResponseBase<IEnumerable<Audio>>.Success(conversations).ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<Audio>>.Error($"{nameof(GetAll)} failed: {ex.Message}").ToActionResult();
            }
        }

        [Authorize]
        [HttpGet("by-course-id/{courseId}")]
        [ProducesResponseType(typeof(ResponseBase<IEnumerable<ConversationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseBase<IEnumerable<ConversationDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCourseId(int courseId, [FromQuery] int? pageSize = null
            , [FromQuery] int? pageNumber = null)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var conversations = await _conversationService.GetByCourseIdAsync(Convert.ToInt32(userId), courseId, pageSize, pageNumber);
                return conversations.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<ConversationDto>>.Error($"{nameof(GetByCourseId)} failed: {ex.Message}").ToActionResult();
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var conversations = await _conversationService.GetByIdAsync(id);
                return conversations.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<ConversationDto>.Error($"{nameof(GetById)} failed: {ex.Message}").ToActionResult();
            }
        }

        [HttpGet("by-conversation-code/{code}")]
        public async Task<IActionResult> GetByConversationCode(string code)
        {
            try
            {
                var conversations = await _conversationService.GetByConversationCodeAsync(code);
                return conversations.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<ConversationDto>.Error($"{nameof(GetByConversationCode)} failed: {ex.Message}").ToActionResult();
            }
        }
    }
}
