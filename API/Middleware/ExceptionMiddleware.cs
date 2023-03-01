using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        // pass the RequestDelegate, ILogger, IHostEnvironment to the constructor
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, 
                IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch(Exception ex)
            {
                // log the errors
                logger.LogError(ex, ex.Message);

                // generate a response in the json format
                httpContext.Response.ContentType = "application/json";

                // specify the status code to return
                httpContext.Response.StatusCode = 500;

                // create a problems details object to return the errors
                var response = new ProblemDetails
                {
                    Status = 500,
                    Detail = env.IsDevelopment() ? ex.StackTrace?.ToString() : null,
                    Title = ex.Message
                };

                // create the JsonSerializerOptions and specify the PropertyNamingPolicy
                var options = new JsonSerializerOptions{ PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
            
                // create a json serializer object and pass the JsonSerializerOptions to it
                var json = JsonSerializer.Serialize(response, options);

                await httpContext.Response.WriteAsync(json);
            }

        }
    }
}