using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Utilities
{
    public class DataTablePage<T>
    {
        public List<T>? Data { get; set; }
        public int TotalRecords { get; set; }
        public int RecordsFiltered { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
