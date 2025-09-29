using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Entities
{
    public class Flashcard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FlashcardCode { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Front { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Furigana { get; set; }

        [StringLength(1000)]
        public string? MeaningVi { get; set; }
        [StringLength(1000)]
        public string? MeaningEn { get; set; }

        [StringLength(2000)]
        public string? Example { get; set; }
        [StringLength(2000)]
        public string? ExampleVi { get; set; }
        [StringLength(2000)]
        public string? ExampleEn { get; set; }

        public string? ImageUrl { get; set; } = string.Empty;

        public int? TopicId { get; set; }
        [ForeignKey(nameof(TopicId))]
        public Topic? Topic { get; set; } = null!;
    }
}
