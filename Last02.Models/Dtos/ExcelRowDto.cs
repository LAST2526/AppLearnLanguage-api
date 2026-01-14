using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Commons;

namespace Last02.Models.Dtos
{
    public class ExcelRowDto<T>
    {
        public string SheetName { get; set; } = string.Empty;
        public string TopicVi { get; set; } = string.Empty;
        public string TopicEn { get; set; } = string.Empty;
        public int RowNumber { get; set; }
        public Language Language { get; set; }
        public T Data { get; set; } = default!;
    }
}
