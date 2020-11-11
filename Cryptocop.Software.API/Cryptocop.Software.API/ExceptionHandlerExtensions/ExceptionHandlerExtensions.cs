
using System;
using System.Net;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Cryptocop.Software.API.ExceptionHandlerExtensions
{
    public static class ExceptionHandlerExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error => {
                error.Run(async context => {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionHandlerFeature.Error;

                    var statusCode = exception switch
                    {
                        ResourceNotFoundException _ => (int) HttpStatusCode.NotFound,
                        ModelFormatException _ => (int) HttpStatusCode.PreconditionFailed,
                        ArgumentOutOfRangeException _ => (int) HttpStatusCode.BadRequest,
                        ConflictException _ => (int) HttpStatusCode.Conflict,
                        _ => (int) HttpStatusCode.InternalServerError
                    };

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = statusCode;

                    await context.Response.WriteAsync(new ExceptionModel
                    {
                        StatusCode = statusCode,
                        ExceptionMessage = exception.Message
                    }.ToString());
                });
            });
        }
    }
}