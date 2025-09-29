using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;

namespace Last02.Data.Repositories.RepositoryBase
{
    public interface IUserRepository : IRepository<Users>
    {
        Task<Users?> GetByEmailAsync(string email);
        Task<Users?> GetByEmailAndDOBAsync(string email, DateTime DOB);
    }
}
