using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Dtos
{
    public class FlashcardDto
    {
        public int Id { get; set; }
        public string FlashcardCode { get; set; } = string.Empty;
        public string Front { get; set; } = string.Empty;
        public string? Furigana { get; set; }
        public string? MeaningVi { get; set; }
        public string? MeaningEn { get; set; }
        public string? Example { get; set; }
        public string? ExampleVi { get; set; }
        public string? ExampleEn { get; set; }
        public string? ImageUrl { get; set; }
        public FlashcardStatus Status { get; set; }
        public int? TopicId { get; set; }
    }
}
