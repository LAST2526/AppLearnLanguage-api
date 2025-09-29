using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Commons;

namespace Last02.Data.Entities
{
    [Table("Audios")]
    public class Audio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AudioCode { get; set; }
        public AudioType? AudioType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        [Required]
        public string FileUrl { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsFree { get; set; } = false;
        public string? Script { get; set; }
        public string? ScriptVi { get; set; }
        public string? ScriptEn { get; set; }
        public int? CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; }
    }
}
