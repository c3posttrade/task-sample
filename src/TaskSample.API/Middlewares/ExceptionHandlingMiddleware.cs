using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaskSample.Api.Models;
using TaskSample.Services.Exceptions;

namespace TaskSample.Api.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                if (ex is NotFoundException)
                {
                    await ErrorResponse(context, ex, HttpStatusCode.NotFound);
                }

                if (ex is ValidationException)
                {
                    await ErrorResponse(context, ex, HttpStatusCode.BadRequest);
                }

                await ErrorResponse(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static async Task ErrorResponse(HttpContext context, Exception ex, HttpStatusCode httpStatusCode)
        {
            context.Response.StatusCode = (int)httpStatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorModel { Message = ex.Message }));
        }
    }
}
