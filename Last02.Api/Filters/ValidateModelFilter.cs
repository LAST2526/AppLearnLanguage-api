using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Last02.Api.Filters
{
    public class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var firstError = context.ModelState
                    .SelectMany(x => x.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault() ?? "Validation failed.";

                var result = new
                {
                    isSuccess = false,
                    message = firstError,
                    data = (object?)null,
                    statusCode = 400
                };

                context.Result = new JsonResult(result) { StatusCode = 400 };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
