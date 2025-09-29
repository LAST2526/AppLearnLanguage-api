using Last02.Commons;
using Last02.Data.Entities;
using Last02.Services.Implement;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Last02.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(ICourseService _courseService) : ControllerBase
    {
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                return ResponseBase<IEnumerable<Course>>.Success(courses).ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<Course>>.Error($"{nameof(GetAll)} failed: {ex.Message}").ToActionResult();
            }
        }
    }
}
