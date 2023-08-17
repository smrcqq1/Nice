using Microsoft.AspNetCore.Http;

namespace Nice.WebAPI
{
    internal class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Nice.Exception exception)
            {
                await ExceptionToResponse(context, exception);
            }
            catch (System.Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }
        public static Func<HttpContext, Exception,Task> DefaultExceptionResponse = HandleExceptionAsync;

        private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            context.Response.ContentType = "application/text";
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(exception.Message);
        }
        private static Task ExceptionToResponse(HttpContext context, Nice.Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.Code;
            return context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(exception.ErrMsg));
        }
    }
}
