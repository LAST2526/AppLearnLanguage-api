using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Dtos
{
    public class UserGeneralInfoResponseDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? RefreshToken { get; set; }
        public int? MemberId { get; set; }
        public string? MemberFullName { get; set; }
        public MemberDto? Member { get; set; }
        public int? MemberCourseId { get; set; }
        public int? CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string? BookInstanceCode { get; set; }
        public bool HasRedeemedBook { get; set; } = false;
        public double CourseProgress { get; set; }
    }
}
