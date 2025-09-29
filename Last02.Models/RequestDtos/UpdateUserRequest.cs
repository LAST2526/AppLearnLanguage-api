using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.RequestDtos
{
    public class UpdateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public Gender? Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public int? CourseId { get; set; }
    }
}
