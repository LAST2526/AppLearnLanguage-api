using Last02.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.UnitOfWork;
using Last02.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Last02.Services.Implement
{
    public class ClaimsService : BaseService, IClaimsService
    {
        private IConfiguration _config;
        public ClaimsService(IUnitOfWork unitOfWork, IConfiguration config) : base(unitOfWork, config)
        {
            _config = config;
        }
        public List<ClaimsViewModel> GetListClaims()
        {

            var claims = UnitOfWork?.Claim.GetAll().Select(x => new ClaimsViewModel()
            {
                Id = x.Id,
                ClaimValue = x.ClaimValue,
                ClaimType = x.ClaimType,

            }).ToList();

            return claims ?? [];
        }
    }
}
