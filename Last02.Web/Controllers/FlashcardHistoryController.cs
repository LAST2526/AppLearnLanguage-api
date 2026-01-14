using Last02.Services.Interfaces;
using Last02.Web.Extensions;
using Last02.Web.Models;
using Last02.Web.Validators;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace Last02.Web.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class FlashcardHistoryController : BaseController
    {
        private readonly ILogger<FlashcardHistoryController> _logger;
        private readonly IFlashcardUpdateHistoryService _flashcardUpdateHistoryService;
        private readonly ICourseService _courseService;
        private readonly IStorageService _storageService;
        private static readonly string[] value = ["Failed to upload flashcard history"];

        public FlashcardHistoryController(IFlashcardUpdateHistoryService flashcardUpdateHistoryService, ICourseService courseService
            , ILogger<FlashcardHistoryController> logger, IStorageService storageService)
        {
            _flashcardUpdateHistoryService = flashcardUpdateHistoryService;
            _courseService = courseService;
            _logger = logger;
            _storageService = storageService;
        }

        public async Task<IActionResult> Index()
        {
            var histories = await _flashcardUpdateHistoryService.GetAllAsync();
            return View(histories);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string keyword, int page, int size)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (size <= 0)
            {
                size = 10;
            }
            var histories = await _flashcardUpdateHistoryService.SearchAsync(keyword, page, size);
            return Json(histories);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(UploadFlashcardViewModel viewModel)
        {
            _logger.LogInformation("Uploading flashcard data");
            // validate view model
            var validator = new UploadFlashcardsValidator(_courseService);
            var validationResult = await validator.ValidateAsync(viewModel);
            var errors = new List<string>();
            if (!validationResult.IsValid)
            {
                var err = validationResult.Errors.ToDictionary();
                return BadRequest(err);
            }

            var topicMap = _flashcardUpdateHistoryService.GetTopicsFromExcel(viewModel.FileContent);
            var flashcards = _flashcardUpdateHistoryService.GetFlashcardsFromExcel(viewModel.FileContent);

            if (!flashcards.Any())
            {
                errors.Add(Resources.Resource.Common_Error_File_Empty);
            }
            var flashcardValidator = new FlashcardValidator();
            foreach (var flashcard in flashcards)
            {
                var flashcardValidationResult = await flashcardValidator.ValidateAsync(flashcard.Data);
                if (!flashcardValidationResult.IsValid)
                {
                    errors.AddRange(flashcardValidationResult.Errors.Select(e => $"シート: {flashcard.SheetName}, 行: {flashcard.RowNumber}, エラー: {e.ErrorMessage}"));
                }
            }
            if (errors.Count > 0)
            {
                var response = new Dictionary<string, string[]>
                {
                    { "Error", errors.ToArray() }
                };
                return BadRequest(response);
            }
            try
            {
                _logger.LogInformation("Creating topic and flashcard model");
                var fileName = Guid.NewGuid().ToString() + ".xlsx";
                var folder = "courses/flashcards";
                var blobName = $"{folder}/{fileName}";

                var file = viewModel.FileContent;
                var contentType = file.ContentType;

                await _flashcardUpdateHistoryService.CreateTopicAndFlashcardModelAsync(
                    topicMap,
                    flashcards,
                    viewModel.CourseIds,
                    blobName,
                    file.FileName
                );

                return Ok(new { message = "Flashcard data uploaded successfully", fileUrl = blobName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var response = new Dictionary<string, string[]>
                {
                    { "Error", value }
                };
                return BadRequest(response);
            }
        }
    }
}
