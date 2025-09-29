using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Models.Dtos;
using Last02.Models.RequestDtos;
using Last02.Models.ResponseDtos;

namespace Last02.Services.Interfaces
{
    public interface IAuthService : IBaseService
    {
        Task<ResponseBase<OAuthLoginDto>> OAuthLogin(OAuthLoginRequest model);
        Task<ResponseBase<EmailLoginDto>> EmailLogin(EmailLoginRequest model);
        Task<ResponseBase<UserDto>> RegisterAndLogin(RegisterAndLoginRequest model);
        Task<ResponseBase<AuthDto>> RefreshTokenAsync(string refreshToken);
        Task<ResponseBase<LogoutDto>> LogoutAsync(int userId, string refreshToken);
        Task<UserGeneralInfoResponseDto?> GetUserInfoAsync(int userId);
    }
}
