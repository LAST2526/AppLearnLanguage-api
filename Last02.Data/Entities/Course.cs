using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Entities
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool Deleted { get; set; } = false;

        // Navigation property for the many-to-many relationship
        public virtual ICollection<MemberCourse> MemberCourses { get; set; } = new List<MemberCourse>();
        public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
        public virtual ICollection<Audio> Audios { get; set; } = new List<Audio>();
    }
}
