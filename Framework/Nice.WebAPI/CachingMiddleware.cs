using Microsoft.AspNetCore.Http;

namespace Nice.WebAPI
{
    internal class CachingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly INiceControllerBuilder builder;

        public CachingMiddleware(RequestDelegate next, INiceControllerBuilder builder)
        {
            this.next = next;
            this.builder = builder;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await next(context);
        }
    }
}