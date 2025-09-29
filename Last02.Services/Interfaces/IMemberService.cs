
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;

namespace Last02.Services.Interfaces
{
    public interface IMemberService : IBaseService
    {
        Member? GetByUserId(int userId);

    }
}
