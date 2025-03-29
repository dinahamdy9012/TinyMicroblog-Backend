﻿using System.Net;
using System.Text.Json;

namespace TinyMicroblog.API.Middlewares
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

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                ApplicationException => HttpStatusCode.BadRequest, // Business logic error
                KeyNotFoundException => HttpStatusCode.NotFound,  // Data not found
                UnauthorizedAccessException => HttpStatusCode.Unauthorized, // Auth errors
                _ => HttpStatusCode.InternalServerError // Other errors
            };

            var response = new
            {
                error = exception.Message,
                statusCode = (int)statusCode
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
