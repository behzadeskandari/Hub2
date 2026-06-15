using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Yarane.Common.ReturnResults;
using Newtonsoft.Json;
using System.Net;
using Yarane.Common.ReturnResults;

namespace Yarane.Common.Exceptions
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next
            )
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                //await _cleanLogger.ErrorLog($"{ex}");
                var apiResult = new ApiResult(
                    false,
                    ex.RsCode,
                    ex.Message
                );

                var json = JsonConvert.SerializeObject(apiResult);

                context.Response.StatusCode = (int)ex.HttpStatusCode;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                //await _cleanLogger.ErrorLog($"{ex}");
                var apiResult = new ApiResult(
                    false,
                    ApiResultRsCode.ServerError,
                    "خطای ناشناخته"
                //ApiResultRsCode.ServerError.ToName()
                );

                var json = JsonConvert.SerializeObject(apiResult);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
        }
    }
    public static class CustomExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
