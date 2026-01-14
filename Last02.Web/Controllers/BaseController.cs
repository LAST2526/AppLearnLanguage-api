using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Last02.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected List<string> GetModelStateErrors()
        {
            return ModelState.Values.SelectMany(v => v.Errors.Select(x => x.ErrorMessage)).ToList();
        }

        protected Guid GetUserIdLogin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userId, out var id);
            return id;
        }
    }
}
