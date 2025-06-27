using Voalaft.API.Constants;
using Voalaft.API.Utils;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;
using System.Text;
using Voalaft.Data.Entidades;

namespace Voalaft.API.Middleware
{
    public class PeticionApiMiddleware
    {

        private readonly RequestDelegate _next;

        public PeticionApiMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            PeticionAPI p = null; 
            try
            {
                string ipAddress = context.Connection.RemoteIpAddress == null ? string.Empty : context.Connection.RemoteIpAddress?.ToString();
                string identifier = ipAddress;
                if (!context.Request.Headers.TryGetValue(ApiConstants.ApiHeaderKey, out var actualApiKey))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Api key No Encontrada.");
                    return;
                }
                context.Request.EnableBuffering();
               using (var reader = new StreamReader(context.Request.Body))
                {
                    var requestBody = await reader.ReadToEndAsync();
                    p = CryptographyUtils.DeserializarPeticion<PeticionAPI>(requestBody);
                    context.Request.Body.Position = 0;
                    context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
                }

                //context.Request.Body = originalBody;
                var hash = CryptographyUtils.EncriptarSha256(p.contenido);
                if (!actualApiKey.Equals((p.hash)))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Api key Invalida.");
                    return;
                }

                if (p != null)
                {
                    context.Items["peticion"] = p;

                }
                await _next(context);
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = (int)400;
                await HandleException(context, ex);
            }
            
        }

        public Task HandleException(HttpContext httpContext, Exception ex)
        {
            string message = "[Error]    HTTP " + httpContext.Request.Method + " - " + httpContext.Response.StatusCode + " Error Message " + ex.Message + " in "
                + "ms";
            Console.WriteLine(message);

            httpContext.Response.ContentType = "application/json";
            if(httpContext.Response.StatusCode != 400 && httpContext.Response.StatusCode != 401)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                message = ex.Message,
            });

            return httpContext.Response.WriteAsync(result);
        }

        public Task HandleException(HttpContext httpContext, HttpStatusCode statusCode, Exception ex)
        {
            string message = "[Error]    HTTP " + httpContext.Request.Method + " - " + httpContext.Response.StatusCode + " Error Message " + ex.Message + " in "
                + "ms";
            Console.WriteLine(message);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)statusCode;

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                message = ex.Message,
            });

            return httpContext.Response.WriteAsync(result);
        }
    }
    public static class PeticionApiMiddlewareExtensions
    {
        public static IApplicationBuilder UsePeticionApiMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PeticionApiMiddleware>();
        }
    }
}
