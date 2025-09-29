using Last02.Commons;
using Last02.Models.Dtos;
using Last02.Models.RequestDtos;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Last02.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILocalizedMessageService _messageService;
        //private readonly IUserDeviceTokenService _userDeviceTokenService;


        public AuthController(IAuthService authService, ILocalizedMessageService messageService)
        {
            _authService = authService;
            _messageService = messageService;
            //_userDeviceTokenService = userDeviceTokenService;
        }

        [HttpPost("oauth-login")]
        public async Task<IActionResult> OAuthLogin([FromBody] OAuthLoginRequest oAuthLoginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.OAuthLogin(oAuthLoginRequest);
                return result.ToActionResult();

            }
            catch (Exception ex)
            {
                return ResponseBase<UserDto>.Error(ex.Message).ToActionResult();
            }
        }

        [HttpPost("email-login")]
        public async Task<IActionResult> EmailLogin([FromBody] EmailLoginRequest emailLoginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.EmailLogin(emailLoginRequest);
                return result.ToActionResult();

            }
            catch (Exception ex)
            {
                return ResponseBase<UserDto>.Error(ex.Message).ToActionResult();
            }
        }

        [HttpPost("register-and-login")]
        public async Task<IActionResult> RegisterAndlLogin([FromBody] RegisterAndLoginRequest registerAndLoginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.RegisterAndLogin(registerAndLoginRequest);
                return result.ToActionResult();

            }
            catch (Exception ex)
            {
                return ResponseBase<UserDto>.Error($"{nameof(RegisterAndlLogin)} failed: {ex.Message}").ToActionResult();
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var userGeneralInfo = await _authService.GetUserInfoAsync(Convert.ToInt32(userId));
                if (userGeneralInfo == null)
                {
                    return NotFound();
                }

                return ResponseBase<UserGeneralInfoResponseDto>.Success(userGeneralInfo).ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<UserGeneralInfoResponseDto>.Error($"{nameof(GetCurrentUser)} failed: {ex.Message}").ToActionResult();
            }
        }
    }
}
