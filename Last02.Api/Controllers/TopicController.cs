using Last02.Commons;
using Last02.Models.Dtos;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Last02.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        //[Authorize]
        [HttpGet("by-course-id/{courseId}")]
        public async Task<IActionResult> GetByCourseId(int courseId, [FromQuery] int? pageSize = null
            , [FromQuery] int? pageNumber = null)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var topics = await _topicService.GetByCourseIdAsync(Convert.ToInt32(userId), courseId, pageSize, pageNumber);
                return topics.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<TopicDto>>.Error(ex.Message).ToActionResult();
            }
        }
    }
}
