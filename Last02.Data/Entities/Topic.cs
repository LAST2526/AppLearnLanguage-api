using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Entities
{
    [Table("Topics")]
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TopicCode { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        public string? HexColorCode { get; set; }
        public bool IsFree { get; set; } = false;
        public int? CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; } = null;
        public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

    }
}
