using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nice.WebAPI
{
    internal class ModelValidator : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        public static Func<string[], object>? ErrorToResultFunc;
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (modelState.IsValid)
            {
                return;
            }
            var errs = modelState.Values.SelectMany(o=> o.Errors).Select(o=> o.ErrorMessage).ToArray();
            if(ErrorToResultFunc == null)
            {
                context.Result = new BadRequestObjectResult(errs);
                return;
            }
            context.Result = new BadRequestObjectResult(ErrorToResultFunc.Invoke(errs));
        }
    }
}