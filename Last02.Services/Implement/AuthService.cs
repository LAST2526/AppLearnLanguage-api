using Amazon.Runtime.Internal.Util;
using Amazon.S3.Model;
using Google.Apis.Auth;
using Last02.Commons;
using Last02.Data.Entities;
using Last02.Data.UnitOfWork;
using Last02.Models;
using Last02.Models.Dtos;
using Last02.Models.RequestDtos;
using Last02.Models.ResponseDtos;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Implement
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUnitOfWork _uow;
        //ILogger<>
        private GoogleAuthSettings _googleAuthSettings = null!;
        private JwtSettings _jwtSettings = null!;
        private IPasswordService _passwordService = null!;
        private ILocalizedMessageService _messageService = null!;
        private IStorageService _storageService = null!;
        private IUserService _userService = null!;


        public AuthService(IUnitOfWork unitOfWork, IOptions<GoogleAuthSettings> googleAuthSettings,
            IOptions<JwtSettings> jwtSettings, IPasswordService passwordService, ILocalizedMessageService messageService, IUserService userService, IStorageService storageService) : base(unitOfWork)
        {
            _uow = unitOfWork;
            _googleAuthSettings = googleAuthSettings.Value;
            _jwtSettings = jwtSettings.Value;
            _passwordService = passwordService;
            _messageService = messageService;
            _userService = userService;
            _storageService = storageService;
        }

        public async Task<ResponseBase<OAuthLoginDto>> OAuthLogin(OAuthLoginRequest model)
        {
            try
            {
                string? email = null;
                string? username = null;
                if (model.Provider == Provider.Google)
                {
                    var payload = await GoogleJsonWebSignature.ValidateAsync(model.SocialProviderToken);
                    var clientIds = _googleAuthSettings.GetAllClientIds();
                    if (!clientIds.Contains(payload.Audience))
                    {
                        return ResponseBase<OAuthLoginDto>.Error(
                            await _messageService.GetMessageAsync(MessageCodes.Auth.ERR_INVALID_GOOGLE_TOKEN), 401);
                    }

                    email = payload.Email;
                    username = payload.Email;
                }

                var refreshToken = "";
                var accessToken = "";
                var existingUser = await _uow.User.GetAsync(
                    u => u.UserName == username && (u.Email == null || u.Email == email) && u.DeletedAt == null &&
                         u.IsActive, include: q => q.Include(u => u.Member));

                Users user = new Users();

                if (existingUser != null)
                {
                    user = existingUser;

                    refreshToken = GenerateRefreshToken();
                    existingUser.RefreshToken = refreshToken;
                    existingUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

                    accessToken = GenerateJwtToken(user, refreshToken);
                    await _uow.SaveAsync();
                }

                return ResponseBase<OAuthLoginDto>.Success(
                    new OAuthLoginDto
                    {
                        Mail = email ?? string.Empty,
                        UserName = username ?? string.Empty,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        IsExist = (existingUser != null)
                    }, message: await _messageService.GetMessageAsync(MessageCodes.Auth.SUC_AUTHENTICATION_SUCCESS));
            }
            catch (Exception ex)
            {
                return ResponseBase<OAuthLoginDto>.Error("System error: " + ex.Message, 500);
            }
        }

        public async Task<ResponseBase<EmailLoginDto>> EmailLogin(EmailLoginRequest model)
        {
            try
            {
                var user = await _uow.User.GetAsync(
                    u => (u.UserName == model.Email) && u.IsActive,
                    include: q => q.Include(u => u.Member)
                );

                if (user == null)
                    return ResponseBase<EmailLoginDto>.Error(await _messageService.GetMessageAsync(MessageCodes.Auth.ERR_USERNAME_NOT_FOUND));

                bool isPasswordValid = _passwordService.VerifyPassword(model.Password, user.PasswordHash ?? "");

                if (!isPasswordValid)
                {
                    return ResponseBase<EmailLoginDto>.Error(await _messageService.GetMessageAsync(MessageCodes.Auth.ERR_PASSWORD_INCORRECT));
                }

                if (user.TemporaryPasswordExpires.HasValue && user.TemporaryPasswordExpires <= DateTime.UtcNow)
                {
                    user.TemporaryPasswordHash = null;
                    user.TemporaryPasswordExpires = null;

                    await _uow.SaveAsync();
                    return ResponseBase<EmailLoginDto>.Error(await _messageService.GetMessageAsync(MessageCodes.Auth.ERR_PASSWORD_INCORRECT));
                }

                var refreshToken = GenerateRefreshToken();
                var accessToken = GenerateJwtToken(user, refreshToken);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
                await _uow.SaveAsync();

                return ResponseBase<EmailLoginDto>.Success(
                    new EmailLoginDto
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        UserName = user.UserName ?? string.Empty,
                        Mail = user.Email ?? string.Empty,
                        IsExist = (user != null)
                    },
                    message: await _messageService.GetMessageAsync(MessageCodes.Auth.SUC_LOGIN_SUCCESS)
                );
            }
            catch (Exception ex)
            {
                return ResponseBase<EmailLoginDto>.Error("System error: " + ex.Message, 500);
            }
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private string GenerateJwtToken(Users user, string refreshToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim("refreshToken", refreshToken ?? "")
            };
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResponseBase<UserDto>> RegisterAndLogin(RegisterAndLoginRequest model)
        {
            try
            {
                if (model.Provider == Provider.None || model.Provider == Provider.Google)
                {
                    model.UserName = model.Email;
                }

                var existingUser = await _uow.User.GetAsync(u => u.UserName == model.UserName && u.Provider == model.Provider);

                if (existingUser != null)
                {
                    return ResponseBase<UserDto>.Error(
                        message: await _messageService.GetMessageAsync(MessageCodes.User.ERR_USER_ALREADY_EXISTS)
                    );
                }

                var existingCourse = await _uow.Course.GetAsync(u => u.Id == model.CourseId);

                if (existingCourse == null)
                {
                    return ResponseBase<UserDto>.Error(
                        message: await _messageService.GetMessageAsync(MessageCodes.User.ERR_COURSE_NOT_EXISTS)
                    );
                }

                await _uow.BeginTransactionAsync();

                var user = await _userService.CreateUserAndMemberAsync(model);
                await _uow.SaveAsync();

                var resultDto = new UserDto();
                if (model.DoLogin.HasValue && model.DoLogin == true)
                {
                    var refreshToken = GenerateRefreshToken();
                    var accessToken = GenerateJwtToken(user, refreshToken);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
                    await _uow.SaveAsync();

                    resultDto = UserService.ConvertToDto(user);
                    resultDto.AccessToken = accessToken;
                }

                await _uow.CommitAsync();

                return ResponseBase<UserDto>.Created(
                    resultDto,
                    message: await _messageService.GetMessageAsync(MessageCodes.User.SUC_USER_CREATED)
                );
            }
            catch (Exception ex)
            {
                return ResponseBase<UserDto>.Error(
                    message: "System error: " + ex.Message,
                    statusCode: 500
                );
            }
        }

        public async Task<ResponseBase<AuthDto>> RefreshTokenAsync(string refreshToken)
        {
            var user = await _uow.User.GetAsync(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return ResponseBase<AuthDto>.Error(await _messageService.GetMessageAsync(MessageCodes.Auth.ERR_INVALID_REFRESH_TOKEN), 401);

            var newRefreshToken = GenerateRefreshToken();
            var newAccessToken = GenerateJwtToken(user, newRefreshToken);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
            await _uow.SaveAsync();

            return ResponseBase<AuthDto>.Success(new AuthDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpireAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes)
            });
        }

        public async Task<ResponseBase<LogoutDto>> LogoutAsync(int userId, string refreshToken)
        {
            try
            {
                var user = await _uow.User.GetAsync(u => u.Id == userId && u.RefreshToken == refreshToken && u.DeletedAt == null && u.IsActive);
                if (user == null)
                {
                    return ResponseBase<LogoutDto>.Error(await _messageService.GetMessageAsync(MessageCodes.Auth.ERR_INVALID_TOKEN_OR_USER_NOT_FOUND), 401);
                }

                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _uow.SaveAsync();

                return ResponseBase<LogoutDto>.Success(new LogoutDto { IsLoggedOut = true }
                    , await _messageService.GetMessageAsync(await _messageService.GetMessageAsync(MessageCodes.Auth.SUC_LOGOUT_SUCCESS)));
            }
            catch (Exception ex)
            {
                return ResponseBase<LogoutDto>.Error("System error: " + ex.Message, 500);
            }
        }

        public async Task<UserGeneralInfoResponseDto?> GetUserInfoAsync(int userId)
        {
            var user = await _uow.User.GetQueryable()
                .Include(u => u.Member)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.Member != null)
            {
                await _uow.Entry(user.Member)
                    .Collection(m => m.MemberCourses)
                    .LoadAsync();
            }

            if (user == null)
                return null;

            var member = user.Member;
            MemberCourse? activeCourse = null;

            double progress = 0;

            if (member != null)
            {
                activeCourse = member.MemberCourses
                    .FirstOrDefault(mc => mc.IsActive);

                if (activeCourse != null)
                {
                    await _uow.Entry(activeCourse)
                        .Reference(mc => mc.Course)
                        .LoadAsync();
                }

                if (activeCourse != null)
                {
                    int totalFlashcardCount = await(
                                    from f in _uow.Flashcard.GetQueryable()
                                    join t in _uow.Topic.GetQueryable() on f.TopicId equals t.Id
                                    join c in _uow.Course.GetQueryable() on t.CourseId equals c.Id
                                    join mc in _uow.MemberCourse.GetQueryable() on c.Id equals mc.CourseId
                                    join m in _uow.Member.GetQueryable() on mc.MemberId equals m.Id
                                    where mc.IsActive && m.Id == member.Id
                                    select f
                                ).CountAsync();

                    int rememberedFlashcardCount = await(
                                    from mf in _uow.MemberFlashcard.GetQueryable()
                                    join f in _uow.Flashcard.GetQueryable() on mf.FlashcardId equals f.Id
                                    join m in _uow.Member.GetQueryable() on mf.MemberId equals m.Id
                                    join t in _uow.Topic.GetQueryable() on f.TopicId equals t.Id
                                    join c in _uow.Course.GetQueryable() on t.CourseId equals c.Id
                                    join mc in _uow.MemberCourse.GetQueryable() on new { CourseId = c.Id, MemberId = m.Id }
                                        equals new { mc.CourseId, mc.MemberId }
                                    where mc.IsActive && mf.Status == FlashcardStatus.Completed && m.Id == member.Id
                                    select mf
                                ).CountAsync();

                    progress = totalFlashcardCount == 0 ? 0 : Math.Round((double)rememberedFlashcardCount / totalFlashcardCount * 100, 2);
                }
            }

            var response = new UserGeneralInfoResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                RefreshToken = user.RefreshToken,
                MemberId = member?.Id,
                MemberFullName = member?.FullName,
                Member = member != null ? MemberService.ConvertToDto(member) : null,
                MemberCourseId = activeCourse?.Id,
                CourseId = activeCourse?.CourseId,
                CourseTitle = activeCourse?.Course?.Title,
                CourseProgress = progress
            };
            if (response.Member != null && !string.IsNullOrEmpty(response.Member.AvatarUrl))
            {
                TimeSpan? timeLeft = user.RefreshTokenExpiryTime - DateTime.UtcNow;
                int validMinutes = 60;
                if (timeLeft.HasValue)
                {
                    validMinutes = Math.Max(1, (int)timeLeft.Value.TotalMinutes);
                }
                //response.Member.AvatarUrl = _storageService.GenerateDownloadUrl(response.Member.AvatarUrl, validMinutes);
            }
            return response;
        }
    }
}
