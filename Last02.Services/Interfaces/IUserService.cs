using Last02.Commons;
using Last02.Data.Entities;
using Last02.Models.Dtos;
using Last02.Models.RequestDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Models.ResponseDtos;

namespace Last02.Services.Interfaces
{
    public interface IUserService : IBaseService
    {
        Task<ResponseBase<UserDto>> GetByIdAsync(int id);
        Task<ResponseBase<UserDto>> GetByUsernameAsync(string username);
        Task<ResponseBase<UserDto>> CreateAsync(CreateUserRequest model);
        Task<ResponseBase<UserDto>> UpdateAsync(int id, UpdateUserRequest model, bool autoSave = true);
        Task<ResponseBase<UpdateAvatarResponseDto>> UpdateAvatarAsync(int id, UpdateAvatarRequest model, bool autoSave = true);

        Task<Users> CreateUserAndMemberAsync(CreateUserRequest model, bool autoSave = true);
        Task<ResponseBase<CheckUserExistDto>> CheckUserExistAsync(string username);
        Task<ResponseBase<ChangePasswordDto>> ChangePasswordAsync(int id, ChangePasswordRequest request);
        Task<ResponseBase<ForgotPasswordDto>> ForgotPasswordAsync(ForgotPasswordRequest request);
    }
}
