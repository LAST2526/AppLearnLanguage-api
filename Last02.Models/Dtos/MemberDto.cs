using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string CourseSelection { get; set; } = string.Empty;
        public int? MemberCourseId { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool MemberLastActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? TimesIsLogoutEnd { get; set; }
        public MemberCourseDto? MemberCourse { get; set; } = default!;
    }
}
