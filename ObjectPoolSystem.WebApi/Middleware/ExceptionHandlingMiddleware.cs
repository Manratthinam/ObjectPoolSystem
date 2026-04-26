using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ObjectPoolSystem.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace ObjectPoolSystem.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (PoolExhaustedException ex)
            {
                _logger.LogError(ex, "Pool exhausted exception occurred.");
                context.Response.StatusCode = 503;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"System fully loaded.\"}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"Internal server error.\"}");
            }
        }
    }
}
