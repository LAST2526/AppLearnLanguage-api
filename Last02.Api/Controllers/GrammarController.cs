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
    public class GrammarController : ControllerBase
    {
        private readonly IGrammarService _grammarService;
        public GrammarController(IGrammarService grammarService)
        {
            _grammarService = grammarService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseBase<IEnumerable<Audio>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseBase<IEnumerable<Audio>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var grammars = await _grammarService.GetAllGrammarsAsync();
                return ResponseBase<IEnumerable<Audio>>.Success(grammars).ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<Audio>>.Error($"{nameof(GetAll)} failed: {ex.Message}").ToActionResult();
            }
        }

        [Authorize]
        [HttpGet("by-course-id/{courseId}")]
        [ProducesResponseType(typeof(ResponseBase<IEnumerable<GrammarDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseBase<IEnumerable<GrammarDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCourseId(int courseId, [FromQuery] int? pageSize = null
            , [FromQuery] int? pageNumber = null)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var grammars = await _grammarService.GetByCourseIdAsync(Convert.ToInt32(userId), courseId, pageSize, pageNumber);
                return grammars.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<GrammarDto>>.Error($"{nameof(GetByCourseId)} failed: {ex.Message}").ToActionResult();
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var grammars = await _grammarService.GetByIdAsync(id);
                return grammars.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<GrammarDto>.Error($"{nameof(GetById)} failed: {ex.Message}").ToActionResult();
            }
        }

        [HttpGet("by-grammar-code/{code}")]
        public async Task<IActionResult> GetByGrammarCode(string code)
        {
            try
            {
                var grammars = await _grammarService.GetByGrammarCodeAsync(code);
                return grammars.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<GrammarDto>.Error($"{nameof(GetByGrammarCode)} failed: {ex.Message}").ToActionResult();
            }
        }
    }
}
