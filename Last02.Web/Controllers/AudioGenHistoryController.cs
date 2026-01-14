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
    public class AudioGenHistoryController(
        IAudioGenHistoryService audioGenQRCodeHistoryService,
        ILogger<AudioGenHistoryController> logger,
        ICourseService courseService,
        IAudioService audioService,
        IStorageService storageService) : BaseController
    {
        private readonly ILogger<AudioGenHistoryController> _logger = logger;
        private readonly IAudioGenHistoryService _audioGenHistoryService = audioGenQRCodeHistoryService;
        private readonly ICourseService _courseService = courseService;
        private readonly IAudioService _audioService = audioService;
        private static readonly string[] value = [Resources.Resource.Common_Error_Upload];
        private readonly IStorageService _storageService = storageService;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string keyword, int page, int size)
        {
            if (page <= 0) page = 1;
            if (size <= 0) size = 10;
            var (data, totalRecords) = await _audioGenHistoryService.SearchAsync(keyword, page, size);
            return Json(new { data, totalRecords, recordsFiltered = totalRecords });
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] UploadCsvAudioViewModel viewModel)
        {
            _logger.LogInformation("Uploading audio data");
            // validate view model
            var validator = new UploadAudiosValidator(_courseService);
            var validationResult = await validator.ValidateAsync(viewModel);
            var errors = new List<string>();
            if (!validationResult.IsValid)
            {
                var err = validationResult.Errors.ToDictionary();
                return BadRequest(err);
            }

            var audios = _audioGenHistoryService.GetAudiosFromExcel(viewModel.FileContent);

            if (!audios.Any())
            {
                errors.Add(Resources.Resource.Common_Error_File_Empty);
            }

            var audioValidator = new AudioValidator();
            foreach (var data in audios)
            {
                var audioValidationResult = await audioValidator.ValidateAsync(data.Data);
                if (!audioValidationResult.IsValid)
                {
                    errors.AddRange(
                        audioValidationResult.Errors.Select(e => $"Cell: {data.RowNumber}, Error: {e.ErrorMessage}"));
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
                _logger.LogInformation("Creating audio model");
                var fileName = $"{Guid.NewGuid()}.xlsx";
                var folder = "audios";
                var file = viewModel.FileContent;

                string key;
                using (var stream = file.OpenReadStream())
                {
                    key = await _storageService.UploadAsync(stream, fileName, file.ContentType, folder, true);
                }

                await _audioGenHistoryService.CreateAudioModelAsync(
                    audios,
                    viewModel.CourseIds,
                    key,
                    file.FileName
                );

                return Ok(new { message = "Audio data uploaded successfully", fileUrl = key });
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