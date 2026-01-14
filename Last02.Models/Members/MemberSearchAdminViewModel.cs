using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Members
{
    public class MemberSearchAdminViewModel : BaseSearchViewModel
    {
        public List<MemberAdminViewModel> Members { get; set; } = [];
        public MemberAdminViewModel? Member { get; set; }
        public bool noti { get; set; }
    }
}
