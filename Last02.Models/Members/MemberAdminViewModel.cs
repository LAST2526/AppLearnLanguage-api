using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Members
{
    public class MemberAdminViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? CourseSelection { get; set; }
        public string? Nationaity { get; set; }
        public int? CourseId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool MemberLastActive { get; set; }
    }
}
