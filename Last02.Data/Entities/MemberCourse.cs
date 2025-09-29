using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Entities
{
    [Table("MemberCourses")]
    public class MemberCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public double? Progress { get; set; }
        public bool IsActive { get; set; }
        [StringLength(500)]
        public string? Notes { get; set; }
        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; } = default!;
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = default!;
    }
}
