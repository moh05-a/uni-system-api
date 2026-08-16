using Microsoft.AspNetCore.Mvc.Filters;

namespace UniSys.Filters
{
    public class LoggingActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            Console.WriteLine("========== BEFORE ACTION ==========");
            Console.WriteLine($"Controller: {controller}");
            Console.WriteLine($"Action: {action}");
            Console.WriteLine($"HTTP Method: {context.HttpContext.Request.Method}");
            Console.WriteLine($"Path: {context.HttpContext.Request.Path}");

            foreach (var argument in context.ActionArguments)
            {
                Console.WriteLine($"{argument.Key}: {argument.Value}");
            }

            Console.WriteLine("===================================");

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("========== AFTER ACTION ==========");

            if (context.Exception == null)
            {
                Console.WriteLine("Action completed successfully.");
            }
            else
            {
                Console.WriteLine($"Exception: {context.Exception.Message}");
            }

            Console.WriteLine("Status Code: " + context.HttpContext.Response.StatusCode);
            Console.WriteLine("==================================");

            base.OnActionExecuted(context);
        }
    }
}