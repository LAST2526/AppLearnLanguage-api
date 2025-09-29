using DocumentFormat.OpenXml.Vml;
using Last02.Commons;
using Last02.Models.Dtos;
using Last02.Models.RequestDtos;
using Last02.Models.ResponseDtos;
using Last02.Services.Implement;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Last02.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("check-user-exist/{username}")]
        public async Task<IActionResult> CheckUserExistAsync(string username)
        {
            try
            {
                var response = await userService.CheckUserExistAsync(username);

                return response.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<CheckUserExistDto>.Error($"{nameof(CheckUserExistAsync)} failed: {ex.Message}").ToActionResult();
            }
        }

        [Authorize]
        [HttpPut("update/{id}")]
        //[ProducesResponseType(typeof(ResponseBase<UserDto>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ResponseBase<UserDto>), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ResponseBase<UserDto>), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(ResponseBase<UserDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await userService.UpdateAsync(id, updateUserRequest);
                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<UserDto>.Error($"{nameof(UpdateUser)} failed: {ex.Message}").ToActionResult();
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await userService.ChangePasswordAsync(Convert.ToInt32(userId), request);
                return response.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<ChangePasswordDto>.Error($"{nameof(ChangePassword)} failed: {ex.Message}").ToActionResult();
            }
        }

        [Authorize]
        [HttpPut("update-avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAvatar([FromForm] UpdateAvatarRequest updateAvatarRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await userService.UpdateAvatarAsync(Convert.ToInt32(userId), updateAvatarRequest);
                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<UserDto>.Error($"{nameof(UpdateAvatar)} failed: {ex.Message}").ToActionResult();
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] Models.RequestDtos.ForgotPasswordRequest request)
        {
            try
            {
                var result = await userService.ForgotPasswordAsync(request);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                return ResponseBase<ForgotPasswordDto>.Error($"{nameof(ForgotPassword)} failed: {ex.Message}").ToActionResult();
            }
        }
    }
}
