using FluentValidation.Results;
using Last02.Models.Dtos;
using Last02.Services.Interfaces;
using Last02.Web.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Last02.Web.Controllers
{
    [Route("{controller}")]
    public class CoursesController(ICourseService courseService,
        ILogger<CoursesController> logger) : BaseController
    {
        private readonly ICourseService _courseService = courseService;
        private readonly ILogger<CoursesController> _logger = logger;
        private readonly CourseCreateValidator _validator = new(courseService);

        public async Task<IActionResult> Index()
        {
            var langs = new List<string> { "vi", "en" };
            ViewBag.CourseLanguagesList = string.Join(",", langs);
            return View();
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string keyword, int page = 1, int size = 100)
        {
            if (size <= 0)
                size = 100;
            if (page <= 0)
                page = 1;

            var courses = await _courseService.AdminSearch(keyword, page, size);
            return Json(courses);
        }

        [HttpPost("ValidateCreate")]
        public async Task<IActionResult> ValidateCreate([FromForm] CourseCreateDto model)
        {
            // Custom validation using FluentValidation
            ValidationResult validationResult = await _validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                _logger.LogError("ValidateCreate: {Errors}", errors);
                return BadRequest(new
                {
                    Message = "Data is not valid",
                    Errors = errors
                });
            }

            _logger.LogInformation("ValidateCreate: {Model}", model);
            return Ok(model);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CourseCreateDto model)
        {
            _logger.LogInformation("Create: {Model}", model);
            try
            {
                // Validate again before creating
                ValidationResult validationResult = await _validator.ValidateAsync(model);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    return BadRequest(new
                    {
                        Message = "Data is not valid",
                        Errors = errors
                    });
                }

                var course = await _courseService.CreateFromDto(model);
                return Ok(course);
            }
            catch (Exception ex)
            {
                _logger.LogError("Create: {Exception}", ex);
                return BadRequest(new
                {
                    Message = "Error creating course",
                    Errors = ex.Message
                });
            }
        }

        [HttpGet("GetByIds")]
        public async Task<IActionResult> GetByIds(string ids)
        {
            _logger.LogInformation("GetByIds: {Ids}", ids);
            var idsArray = ids.Split(',').Select(int.Parse).ToArray();
            var course = await _courseService.AdminGetByIds(idsArray);
            if (course == null)
            {
                return BadRequest(new
                {
                    Message = "Course not found",
                    Errors = "Course not found"
                });
            }

            _logger.LogInformation("GetByIds: {Course}", course);
            var sourceDto = course.GroupBy(c => c.Title).Select(g => new CourseCreateDto
            {
                Id = string.Join(",", g.Select(c => c.Id)),
                Title = g.Key ?? string.Empty,
                CreatedDate = g.Select(c => c.CreatedDate).First(),
            }).FirstOrDefault();
            return Ok(sourceDto);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm] CourseCreateDto model)
        {
            _logger.LogInformation("Update: {Model}", model);
            try
            {
                var course = await _courseService.UpdateFromDto(model);
                return Ok(course);
            }
            catch (Exception ex)
            {
                _logger.LogError("Update: {Exception}", ex);
                return BadRequest(new
                {
                    Message = "Error updating course",
                    Errors = ex.Message
                });
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int[] ids)
        {
            _logger.LogInformation("Delete: {Ids}", ids);
            await _courseService.Delete(ids);
            return Ok(new { Message = "Courses deleted successfully" });
        }

        [HttpGet("GetCourses")]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            var courseDtos = courses.GroupBy(c => c.Title).Select(g => new CourseCreateDto
            {
                Id = string.Join(",", g.Select(c => c.Id)),
                Title = g.Key ?? string.Empty,
                CreatedDate = g.Select(c => c.CreatedDate).First()
            }).ToList();
            return Json(courseDtos);
        }
    }
}
