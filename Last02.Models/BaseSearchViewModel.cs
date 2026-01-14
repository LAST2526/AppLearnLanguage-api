using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models
{
    public class BaseSearchViewModel
    {
        public string Keyword { get; set; } = string.Empty;

        public PagerViewModel? Pager { get; set; }
        public OrderModel? Order { get; set; }
    }
}
