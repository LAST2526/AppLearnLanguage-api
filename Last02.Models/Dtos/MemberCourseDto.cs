using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Dtos
{
    public class MemberCourseDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public double? Progress { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public CourseDto? Course { get; set; } = new CourseDto();
    }
}
