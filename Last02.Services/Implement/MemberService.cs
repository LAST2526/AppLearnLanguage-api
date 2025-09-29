
using Last02.Data.UnitOfWork;
using Last02.Models.Dtos;
using Last02.Services.Excptions;
using Last02.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;

namespace Last02.Services.Implement
{
    public class MemberService : BaseService, IMemberService
    {
        ILogger<MemberService> _logger;
        IUnitOfWork _uow;
        private IStorageService _storageService = null!;

        public MemberService(IUnitOfWork unitOfWork, ILogger<MemberService> logger
            , IStorageService storageService) : base(unitOfWork)
        {
            _uow = unitOfWork;
            _logger = logger;
            _storageService = storageService;
        }

        public static MemberDto ConvertToDto(Last02.Data.Entities.Member member)
        {
            return new MemberDto
            {
                Id = member.Id,
                UserId = member.UserId,
                RoleId = member.RoleId,
                FullName = member.FullName,
                AvatarUrl = member.AvatarUrl,
                Gender = member.Gender,
                DOB = member.DOB,
                Nationality = member.Nationality,
                IsActive = member.IsActive,
                UpdatedAt = member.UpdatedAt,
                DeletedAt = member.DeletedAt,
                MemberLastActive = member.MemberLastActive,
                LastLoginAt = member.LastLoginAt,
                TimesIsLogoutEnd = member.TimesIsLogoutEnd,
            };
        }

        public Member? GetByUserId(int userId)
        {
            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }
            return UnitOfWork.Member.GetAll().FirstOrDefault(x => x.UserId == userId);
        }
    }
}
