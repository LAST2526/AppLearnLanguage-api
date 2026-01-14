using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Models;

namespace Last02.Services.Interfaces
{
    public interface IClaimsService
    {
        List<ClaimsViewModel> GetListClaims();
    }
}
