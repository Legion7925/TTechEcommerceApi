﻿using Newtonsoft.Json;
using Serilog;
using System.Net;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Shared.Model;

namespace TTechEcommerceApi.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
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
            catch (TTechException te)
            {
                await HandleCustomExceptionAsync(context, te);
            }
            catch (Exception error)
            {
                await HandleUnhandledExceptionAsync(context, error);
            }
        }

        public async Task HandleCustomExceptionAsync(HttpContext context, TTechException te)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsJsonAsync(new ErrorResponse
            {
                Message = te.Message,
                StatusCode = (int)HttpStatusCode.BadRequest,
            });
        }

        private async Task HandleUnhandledExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsJsonAsync(new ErrorResponse
            {
                Message = "Something went wrong please contact our support",
                StatusCode = (int)HttpStatusCode.InternalServerError,
            });
        }
    }
}
