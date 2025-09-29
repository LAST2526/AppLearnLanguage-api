using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Dtos
{
    public class AudioDto
    {
        public int Id { get; set; }
        public string AudioCode { get; set; } = string.Empty;
        public AudioType AudioType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsFree { get; set; }
        public string? Script { get; set; }
        public string? ScriptVi { get; set; }
        public string? ScriptEn { get; set; }
        public string? QrCode { get; set; }
        public int? CourseId { get; set; }
    }
}
