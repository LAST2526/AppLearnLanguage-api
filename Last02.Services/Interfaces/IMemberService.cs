
using Last02.Data.Entities;
using Last02.Models;
using Last02.Models.Members;
using Last02.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Interfaces
{
    public interface IMemberService : IBaseService
    {
        Member? GetByUserId(int userId);
        List<MemberAdminViewModel> Search(MemberSearchView memberSearch, string searchText,
       string orderField, bool IsAscOrder, int page, int size, out int total);
        List<Member> Search(string keyword);
        List<Member> GetByIds(List<int> ids);
        Member? GetById(int id);
        void Update(Member member);
        void Delete(int id);
        void Activate(int id);
        void Deactivate(int id);
        Task<DataTablePage<MemberAdminViewModel>> AdminSearch(MemberSearchView memberSearch, string searchText,
        List<OrderModel> orders, int page, int size);
    }
}
