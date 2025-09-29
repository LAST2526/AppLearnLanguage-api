using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;
using Last02.Data.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;

namespace Last02.Data.Repositories.Implements
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower());
        }

        public async Task<Users?> GetByEmailAndDOBAsync(string email, DateTime dob)
        {
            return await _context.Users
                .Where(u => u.Email != null && u.Email.ToLower() == email.ToLower()
                                            && u.Member != null && u.Member.DOB.Date == dob.Date)
                .Include(u => u.Member)
                .FirstOrDefaultAsync();
        }
    }
}
