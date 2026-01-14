using Hangfire.Dashboard;

namespace Last02.Web.Filters
{
    public class CookieHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User?.Identity?.IsAuthenticated == true;
        }
    }
}
