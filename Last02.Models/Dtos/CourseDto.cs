using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Dtos
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
    }
}
