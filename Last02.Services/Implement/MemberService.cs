using Last02.Data.Entities;
using Last02.Data.UnitOfWork;
using Last02.Models;
using Last02.Models.Dtos;
using Last02.Models.Members;
using Last02.Services.Excptions;
using Last02.Services.Interfaces;
using Last02.Services.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<MemberAdminViewModel> Search(MemberSearchView memberSearch, string searchText, string orderField,
            bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Member service Search={searchText}, Page={page}");
            int? parsedId = null;
            if (!string.IsNullOrEmpty(searchText) && int.TryParse(searchText, out int tempId))
            {
                parsedId = tempId;
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.Trim().ToLower();
            }

            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }

            var query = UnitOfWork.Member.GetAll().Include(x => x.User).Where(x => x.DeletedAt == null);
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.FullName.ToLower().Contains(searchText)
                                         || (x.User != null && x.User.Email != null &&
                                             x.User.Email.ToLower().Contains(searchText)));
            }

            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "fullname":
                        query = IsAscOrder ? query.OrderBy(x => x.FullName) : query.OrderByDescending(x => x.FullName);
                        break;
                    case "email":
                        query = IsAscOrder
                            ? query.OrderBy(x => x.User != null && x.User.Email != null ? x.User.Email : string.Empty)
                            : query.OrderByDescending(x =>
                                x.User != null && x.User.Email != null ? x.User.Email : string.Empty);
                        break;
                }
            }

            total = query.Count();
            if (page <= 0) page = 1;
            if (size <= 0) size = 100;
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            var data = datas.Select(item =>
            {
                var activeMemberCourse = item.MemberCourses?.FirstOrDefault(mc => mc.IsActive);

                return new MemberAdminViewModel
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    FullName = item.FullName,
                    Email = item.User?.Email ?? string.Empty,
                    Gender = item.Gender,
                    DOB = item.DOB,
                    Nationaity = item.Nationality,
                    CourseId = activeMemberCourse?.CourseId,
                    IsActive = item.IsActive,
                    UpdatedAt = item.UpdatedAt
                };
            }).ToList();

            return data;
        }

        public List<Member> Search(string keyword)
        {
            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }

            var query = UnitOfWork.Member.GetAll().Include(x => x.MemberCourses)
                .ThenInclude(mc => mc.Course).Include(x => x.User).Where(x => x.DeletedAt == null);
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(x => x.FullName.ToLower().Contains(keyword)
                                         || (x.User != null && x.User.Email != null &&
                                             x.User.Email.ToLower().Contains(keyword)));
            }

            return [.. query];
        }

        public List<Member> GetByIds(List<int> ids)
        {
            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }

            return
            [
                .. UnitOfWork.Member.GetAll().Include(x => x.MemberCourses)
                    .ThenInclude(mc => mc.Course).Where(x => ids.Contains(x.Id))
            ];
        }

        public Member? GetById(int id)
        {
            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }

            return UnitOfWork.Member.GetAll().FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        }

        public void Update(Member member)
        {
            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }

            UnitOfWork.Member.Update(member);
            UnitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }

            var member = GetById(id) ?? throw new MemberNotFoundException("Member not found");
            member.DeletedAt = DateTime.Now;
            UnitOfWork.Member.Update(member);
            UnitOfWork.SaveChanges();
        }

        public void Activate(int id)
        {
            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }

            var member = GetById(id) ?? throw new MemberNotFoundException("Member not found");
            member.IsActive = true;
            UnitOfWork.Member.Update(member);
            UnitOfWork.SaveChanges();
        }

        public void Deactivate(int id)
        {
            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }

            var member = GetById(id) ?? throw new MemberNotFoundException("Member not found");
            member.IsActive = false;
            UnitOfWork.Member.Update(member);
            UnitOfWork.SaveChanges();
        }

        public async Task<DataTablePage<MemberAdminViewModel>> AdminSearch(MemberSearchView memberSearch,
            string searchText, List<OrderModel> orders, int page, int size)

        {
            _logger.LogDebug($"Get all Member service Search={searchText}, Page={page}");
            int? parsedId = null;
            if (!string.IsNullOrEmpty(searchText) && int.TryParse(searchText, out int tempId))
            {
                parsedId = tempId;
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.Trim().ToLower();
            }

            if (UnitOfWork == null)
            {
                throw new UnitOfWorkNullException("UnitOfWork is null");
            }

            var query = UnitOfWork.Member.GetAll().Include(x => x.User).Include(x => x.MemberCourses)
                .ThenInclude(mc => mc.Course).Where(x => x.DeletedAt == null);
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.FullName.ToLower().Contains(searchText)
                                         || (x.User != null && x.User.Email != null &&
                                             x.User.Email.ToLower().Contains(searchText)));
            }

            if (orders != null)
            {
                foreach (var orderField in orders)
                {
                    switch (orderField.column.ToLower())
                    {
                        case "userid":
                            query = orderField.dir == "asc"
                                ? query.OrderBy(x => x.Id)
                                : query.OrderByDescending(x => x.Id);
                            break;
                        case "fullname":
                            query = orderField.dir == "asc"
                                ? query.OrderBy(x => x.FullName)
                                : query.OrderByDescending(x => x.FullName);
                            break;
                        case "email":
                            query = orderField.dir == "asc"
                                ? query.OrderBy(x =>
                                    x.User != null && x.User.Email != null ? x.User.Email : string.Empty)
                                : query.OrderByDescending(x =>
                                    x.User != null && x.User.Email != null ? x.User.Email : string.Empty);
                            break;
                    }
                }
            }

            if (page <= 0) page = 1;
            if (size <= 0) size = 100;
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            var data = datas.Select(item =>
            {
                var activeMemberCourse = item.MemberCourses?.FirstOrDefault(mc => mc.IsActive);

                return new MemberAdminViewModel
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    FullName = item.FullName,
                    Email = item.User?.Email ?? string.Empty,
                    Gender = item.Gender,
                    DOB = item.DOB,
                    Nationaity = item.Nationality,
                    CourseId = activeMemberCourse?.CourseId,
                    CourseSelection = activeMemberCourse?.Course.Title,
                    IsActive = item.IsActive,
                    UpdatedAt = item.UpdatedAt,
                    CreatedAt = item.User?.CreatedAt
                };
            }).ToList();

            return await Task.FromResult(new DataTablePage<MemberAdminViewModel>
            {
                Data = data,
                TotalRecords = query.Count(),
                RecordsFiltered = query.Count(),
                TotalPages = (int)Math.Ceiling(query.Count() / (double)size),
                Page = page,
                Size = size
            });
        }
    }
}