using Amazon.S3.Model;
using DocumentFormat.OpenXml.Spreadsheet;
using Last02.Commons;
using Last02.Data.Entities;
using Last02.Data.Identity.Models;
using Last02.Data.UnitOfWork;
using Last02.Models;
using Last02.Models.Dtos;
using Last02.Models.RequestDtos;
using Last02.Models.ResponseDtos;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Members = Last02.Data.Entities.Member;
using Users = Last02.Data.Entities.Users;

namespace Last02.Services.Implement
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUnitOfWork _uow;
        ILogger<UserService> _logger;
        private CloudinaryService _cloudinaryService;

        private IPasswordService _passwordService = null!;

        private IMailService _mailService = null!;
        private ILocalizedMessageService _messageService = null!;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger,
            IOptions<GoogleAuthSettings> googleAuthSettings
            , IOptions<JwtSettings> jwtSettings, IPasswordService passwordService
            , ILocalizedMessageService messageService, IStorageService storageService, CloudinaryService cloudinaryService, IMailService mailService) : base(unitOfWork)
        {
            _uow = unitOfWork;
            _logger = logger;
            _passwordService = passwordService;
            _messageService = messageService;
            _cloudinaryService = cloudinaryService;
            _mailService = mailService;
        }

        public async Task<ResponseBase<UserDto>> GetByIdAsync(int id)
        {
            try
            {
                var user = await _uow.User.GetAsync(
                    predicate: u => u.Id == id,
                    include: source => source
                        .Include(u => u.Member)
                        .Include(u => u.UserRoles));

                if (user == null)
                {
                    return ResponseBase<UserDto>.Error(await _messageService.GetMessageAsync(MessageCodes.User.ERR_USER_NOT_FOUND), statusCode: 404);
                }

                var dto = ConvertToDto(user);

                return ResponseBase<UserDto>.Success(dto, await _messageService.GetMessageAsync(MessageCodes.User.SUC_USER_RETRIEVED));
            }
            catch (Exception ex)
            {
                return ResponseBase<UserDto>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        public async Task<ResponseBase<UserDto>> GetByUsernameAsync(string username)
        {
            try
            {
                var user = await _uow.User.GetAsync(
                    predicate: u => u.UserName == username,
                    include: source => source
                        .Include(u => u.Member)
                        .Include(u => u.UserRoles));
                if (user == null)
                {
                    return ResponseBase<UserDto>.Error(await _messageService.GetMessageAsync(MessageCodes.User.ERR_USER_NOT_FOUND), statusCode: 404);
                }

                var dto = ConvertToDto(user);

                return ResponseBase<UserDto>.Success(dto, await _messageService.GetMessageAsync(MessageCodes.User.SUC_USER_RETRIEVED));
            }
            catch (Exception ex)
            {
                return ResponseBase<UserDto>.Error("System error: " + ex.Message, statusCode: 500);
            }
        }

        public async Task<ResponseBase<UserDto>> CreateAsync(CreateUserRequest model)
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

                var result = await CreateUserAndMemberAsync(model);

                return ResponseBase<UserDto>.Created(
                    ConvertToDto(result),
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

        [Authorize]
        public async Task<ResponseBase<UserDto>> UpdateAsync(int id, UpdateUserRequest model, bool autoSave = true)
        {
            try
            {
                var user = await _uow.User.GetAsync(u => u.Id == id);
                if (user == null)
                {
                    return ResponseBase<UserDto>.Error(await _messageService.GetMessageAsync(MessageCodes.User.ERR_USER_NOT_FOUND));
                }

                user.UpdatedAt = DateTime.UtcNow;
                _uow.User.Update(user);

                var member = await _uow.Member.GetAsync(m => m.UserId == id && m.DeletedAt == null && m.IsActive);
                if (member != null)
                {
                    member.UpdatedAt = DateTime.UtcNow;
                    member.FullName = model.Name;
                    member.Nationality = model.Nationality;
                    member.DOB = model.DOB;
                    member.Gender = model.Gender!.Value;
                    member.IsActive = true;
                    member.MemberLastActive = true;

                    if (model.CourseId.HasValue)
                    {
                        var course = await _uow.Course.GetAllAsync(c => c.Id == model.CourseId.Value);
                        if (course == null || course.Count == 0)
                        {
                            return ResponseBase<UserDto>.Error(
                                message: await _messageService.GetMessageAsync(MessageCodes.User.ERR_COURSE_NOT_EXISTS)
                            );
                        }
                        var memberCourses = await _uow.MemberCourse.GetAllAsync(mc => mc.MemberId == member.Id);

                        // Set all to inactive
                        foreach (var memberCourse in memberCourses)
                        {
                            memberCourse.IsActive = false;
                        }

                        // Check if course already exists
                        var existingCourse = memberCourses.FirstOrDefault(mc => mc.CourseId == model.CourseId.Value);
                        if (existingCourse != null)
                        {
                            existingCourse.EnrollmentDate = DateTime.UtcNow;
                            existingCourse.IsActive = true;
                        }
                        else
                        {
                            var newMemberCourse = new MemberCourse
                            {
                                MemberId = member.Id,
                                CourseId = model.CourseId.Value,
                                EnrollmentDate = DateTime.UtcNow,
                                IsActive = true,
                                Progress = 0
                            };
                            await _uow.MemberCourse.AddAsync(newMemberCourse);
                        }
                    }
                }

                if (autoSave)
                {
                    await _uow.SaveAsync();
                }

                var result = user;
                result.Member = member;
                return ResponseBase<UserDto>.Success(
                    ConvertToDto(result),
                    message: await _messageService.GetMessageAsync(MessageCodes.User.SUC_USER_UPDATED)
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

        [Authorize]
        public async Task<ResponseBase<UpdateAvatarResponseDto>> UpdateAvatarAsync(int id, UpdateAvatarRequest model, bool autoSave = true)
        {
            try
            {
                var user = await _uow.User.GetAsync(u => u.Id == id);
                if (user == null)
                {
                    return ResponseBase<UpdateAvatarResponseDto>.Error(
                        await _messageService.GetMessageAsync(MessageCodes.User.ERR_USER_NOT_FOUND));
                }

                user.UpdatedAt = DateTime.UtcNow;
                _uow.User.Update(user);

                var member = await _uow.Member.GetAsync(m => m.UserId == id && m.DeletedAt == null && m.IsActive);
                if (member != null)
                {
                    if (model.AvatarImage != null && model.AvatarImage.Length > 0)
                    {
                        // Xóa avatar cũ trên Cloudinary nếu có
                        if (!string.IsNullOrEmpty(member.AvatarUrl))
                        {
                            try
                            {
                                var publicId = _cloudinaryService.GetPublicIdFromUrl(member.AvatarUrl);
                                await _cloudinaryService.DeleteImageAsync(publicId);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"Không thể xóa avatar cũ trên Cloudinary: {ex.Message}");
                            }
                        }

                        // Upload avatar mới
                        var uploadResult = await _cloudinaryService.UploadImageAsync(model.AvatarImage);
                        if (uploadResult.StatusCode == HttpStatusCode.OK)
                        {
                            member.AvatarUrl = uploadResult.SecureUrl.AbsoluteUri;
                            member.UpdatedAt = DateTime.UtcNow;
                        }
                        else
                        {
                            return ResponseBase<UpdateAvatarResponseDto>.Error("Upload avatar thất bại");
                        }
                    }
                }

                if (autoSave)
                {
                    await _uow.SaveAsync();
                }

                var result = new UpdateAvatarResponseDto
                {
                    AvatarUrl = member?.AvatarUrl
                };

                return ResponseBase<UpdateAvatarResponseDto>.Success(
                    result,
                    message: await _messageService.GetMessageAsync(MessageCodes.User.SUC_USER_UPDATED)
                );
            }
            catch (Exception ex)
            {
                return ResponseBase<UpdateAvatarResponseDto>.Error(
                    message: "System error: " + ex.Message,
                    statusCode: 500
                );
            }
        }

        public async Task<Users> CreateUserAndMemberAsync(CreateUserRequest model, bool autoSave = true)
        {
            try
            {
                // Create User
                var newUser = new Users()
                {
                    Provider = model.Provider!.Value,
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = false,
                    PasswordHash = _passwordService.HashPassword(model.Password),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    UserType = UserType.User
                };
                await _uow.User.AddAsync(newUser);

                // Create Member
                var newMember = new Members()
                {
                    User = newUser,
                    RoleId = 2,
                    FullName = model.Name,
                    Nationality = model.Nationality,
                    DOB = model.DOB?.Date ?? default,
                    Gender = model.Gender!.Value,
                    AvatarUrl = model.AvatarUrl,
                    IsActive = true,
                    MemberLastActive = true,
                    UpdatedAt = DateTime.UtcNow
                };
                await _uow.Member.AddAsync(newMember);

                // Create active MemberCourse (only one active at a time)
                if (model.CourseId.HasValue)
                {
                    var memberCourse = new MemberCourse
                    {
                        Member = newMember,
                        CourseId = model.CourseId.Value,
                        EnrollmentDate = DateTime.UtcNow,
                        IsActive = true,
                        Progress = 0
                    };

                    await _uow.MemberCourse.AddAsync(memberCourse);
                }

                if (autoSave)
                {
                    await _uow.SaveAsync();
                }

                var result = newUser;
                result.Member = newMember;
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseBase<CheckUserExistDto>> CheckUserExistAsync(string username)
        {
            try
            {
                var existingUser = await _uow.User.GetAsync(u =>
                    u.UserName == username
                );

                if (existingUser != null)
                {
                    return ResponseBase<CheckUserExistDto>.Success(
                        data: new CheckUserExistDto()
                        {
                            IsExist = true
                        },
                        message: await _messageService.GetMessageAsync(MessageCodes.User.ERR_USER_ALREADY_EXISTS)
                    );
                }
                else
                {
                    return ResponseBase<CheckUserExistDto>.Success(
                        data: new CheckUserExistDto()
                        {
                            IsExist = false
                        },
                        message: await _messageService.GetMessageAsync(MessageCodes.User.ERR_USER_NOT_FOUND)
                    );
                }
            }
            catch (Exception ex)
            {
                return ResponseBase<CheckUserExistDto>.Error(
                    message: "System error: " + ex.Message,
                    statusCode: 500
                );
            }
        }

        public async Task<ResponseBase<ChangePasswordDto>> ChangePasswordAsync(int id, ChangePasswordRequest request)
        {
            try
            {
                var user = await _uow.User.GetQueryable().FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                    return ResponseBase<ChangePasswordDto>.Error(
                        message: await _messageService.GetMessageAsync(MessageCodes.User.ERR_USER_NOT_FOUND)
                    );

                if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
                    return ResponseBase<ChangePasswordDto>.Error(
                        message: await _messageService.GetMessageAsync(MessageCodes.User.ERR_OLD_PASSWORD_INCORRECT)
                    );

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.TemporaryPasswordHash = null;
                user.TemporaryPasswordExpires = null;
                _uow.User.Update(user);
                await _uow.SaveAsync();

                return ResponseBase<ChangePasswordDto>.Success(
                    new ChangePasswordDto()
                    {
                        IsChanged = true
                    },
                    message: await _messageService.GetMessageAsync(MessageCodes.User.SUC_PASSWORD_UPDATED)
                );
            }
            catch (Exception ex)
            {
                return ResponseBase<ChangePasswordDto>.Error(
                    message: "System error: " + ex.Message,
                    statusCode: 500
                );
            }
        }

        public async Task<ResponseBase<ForgotPasswordDto>> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            try
            {
                var user = await _uow.User.GetByEmailAndDOBAsync(request.Email, request.DOB!.Value);
                if (user == null)
                    return ResponseBase<ForgotPasswordDto>.Error(await _messageService.GetMessageAsync(MessageCodes.User.ERR_MAIL_DOB_NOT_MATCH));

                if (user.LastForgotPasswordRequestAt.HasValue && user.LastForgotPasswordRequestAt.Value.AddMinutes(5) > DateTime.UtcNow)
                {
                    return ResponseBase<ForgotPasswordDto>.Error(await _messageService.GetMessageAsync(MessageCodes.User.ERR_RESET_PASSWORD_TOO_OFTEN));
                }

                var newPassword = GenerateRandomPassword();

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.TemporaryPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.TemporaryPasswordExpires = DateTime.UtcNow.AddMinutes(5);
                user.LastForgotPasswordRequestAt = DateTime.UtcNow;

                var member = await _uow.Member.GetAsync(m => m.UserId == user.Id);
                if (member == null)
                    return ResponseBase<ForgotPasswordDto>.Error(await _messageService.GetMessageAsync(MessageCodes.User.ERR_USER_NOT_FOUND));

                var subject = await _messageService.GetMessageAsync(MessageCodes.Common.EMAIL_TEMP_PASSWORD_SUBJECT);
                var bodyTemplate = await _messageService.GetMessageAsync(MessageCodes.Common.EMAIL_TEMP_PASSWORD_BODY);
                var body = string.Format(bodyTemplate, member.FullName, newPassword);

                var mailData = new MailData
                {
                    ReceiverMailAddr = user.Email!,
                    ReceiverMailName = member?.FullName ?? user.Email!,
                    EmailSubject = subject,
                    EmailBody = body
                };

                var sent = _mailService.SendMail(mailData, true);
                if (!sent)
                    return ResponseBase<ForgotPasswordDto>.Error(await _messageService.GetMessageAsync(MessageCodes.User.ERR_FAILED_TO_SEND_EMAIL));

                _uow.User.Update(user);
                await _uow.SaveAsync();

                return ResponseBase<ForgotPasswordDto>.Success(
                    new ForgotPasswordDto
                    {
                        IsSent = true
                    },
                    await _messageService.GetMessageAsync(MessageCodes.User.SUC_TEMP_PASSWORD_SENT));
            }
            catch (Exception ex)
            {
                return ResponseBase<ForgotPasswordDto>.Error(
                    message: "System error: " + ex.Message,
                    statusCode: 500);
            }
        }

        private string GenerateRandomPassword(int length = 10)
        {
            if (length < 6) length = 6;

            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string special = "!@#$%^&*";

            var random = new Random();

            var passwordChars = new List<char>
            {
                lowercase[random.Next(lowercase.Length)],
                uppercase[random.Next(uppercase.Length)],
                digits[random.Next(digits.Length)],
                special[random.Next(special.Length)]
            };

            var allChars = lowercase + uppercase + digits + special;
            for (int i = passwordChars.Count; i < length; i++)
            {
                passwordChars.Add(allChars[random.Next(allChars.Length)]);
            }

            return new string(passwordChars.OrderBy(_ => random.Next()).ToArray());
        }

        public static UserDto ConvertToDto(Users user)
        {
            var activeMemberCourse = user.Member?.MemberCourses?.FirstOrDefault(mc => mc.IsActive);

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Mail = user.Email ?? string.Empty,
                RefreshToken = user.RefreshToken ?? string.Empty,
                Provider = user.Provider,
                UserType = user.UserType,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                DeletedAt = user.DeletedAt,
                RoleIds = user.UserRoles?.Select(r => r.RoleId).ToList() ?? new List<int>(),

                Member = user.Member == null
                    ? null
                    : new MemberDto
                    {
                        Id = user.Member.Id,
                        UserId = user.Member.UserId,
                        RoleId = user.Member.RoleId,
                        FullName = user.Member.FullName,
                        AvatarUrl = user.Member.AvatarUrl,
                        Gender = user.Member.Gender,
                        DOB = user.Member.DOB,
                        Nationality = user.Member.Nationality,
                        IsActive = user.Member.IsActive,
                        UpdatedAt = user.Member.UpdatedAt,
                        DeletedAt = user.Member.DeletedAt,
                        MemberLastActive = user.Member.MemberLastActive,
                        LastLoginAt = user.Member.LastLoginAt,
                        TimesIsLogoutEnd = user.Member.TimesIsLogoutEnd,

                        MemberCourse = activeMemberCourse == null
                            ? null
                            : new MemberCourseDto
                            {
                                Id = activeMemberCourse.Id,
                                CompletionDate = activeMemberCourse.CompletionDate,
                                MemberId = activeMemberCourse.MemberId,
                                CourseId = activeMemberCourse.CourseId,
                                EnrollmentDate = activeMemberCourse.EnrollmentDate,
                                Progress = activeMemberCourse.Progress,
                                IsActive = activeMemberCourse.IsActive,
                                Notes = activeMemberCourse.Notes,
                                Course = activeMemberCourse.Course == null
                                    ? null
                                    : new CourseDto
                                    {
                                        Id = activeMemberCourse.Course.Id,
                                        Title = activeMemberCourse.Course.Title ?? string.Empty,
                                        CreatedDate = activeMemberCourse.Course.CreatedDate,
                                    }
                            }
                    }
            };
        }
    }
}