using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// RESTful 风格返回值
    /// </summary>
    [SkipScan, UnifyResultModel(typeof(RESTfulResult<>))]
    public class RESTfulResultProvider : IUnifyResultProvider
    {
        /// <summary>
        /// 异常返回值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="metadata">异常元数据</param>
        /// <returns></returns>
        public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
        {
           // return new JsonResult(RESTfulResult(metadata.StatusCode, data: metadata.Data, errors: metadata.Errors)
           //, UnifyContext.GetSerializerSettings(context));
           // // 解析异常信息
           // var (StatusCode, _, Message) = UnifyResultContext.GetExceptionMetadata(context);

            return new JsonResult(new RESTfulResult<object>
            {
                StatusCode = metadata.StatusCode,
                Succeeded = false,
                Data = metadata.Data,
                ErrorCode = metadata.ErrorCode,
                Errors = metadata.Errors,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
        }

        /// <summary>
        /// 成功返回值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IActionResult OnSucceeded(ResultExecutingContext context)
        {
            object data;
            if(context.Result is EmptyResult) data = null;
            // 处理内容结果
            else if (context.Result is ContentResult contentResult) data = contentResult.Content;
            // 处理对象结果
            else if (context.Result is ObjectResult objectResult)
            {
                data = objectResult.Value;
                if (objectResult.DeclaredType == typeof(RESTfulResult<>))
                {
                    return context.Result;
                }
            }
            else return null;

            return new JsonResult(new RESTfulResult<object>
            {
                StatusCode = context.Result is EmptyResult ? StatusCodes.Status204NoContent : StatusCodes.Status200OK,  // 处理没有返回值情况 204
                Succeeded = true,
                Data = data,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
        }


        /// <summary>
        /// 拦截返回状态码
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public async Task OnResponseStatusCodes(HttpContext context, int statusCode)
        {
            switch (statusCode)
            {
                // 处理 401 状态码
                case StatusCodes.Status401Unauthorized:
                    context.Response.ContentType = "application/json";
                    //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsJsonAsync(new RESTfulResult<object>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        ErrorCode = StatusCodes.Status401Unauthorized,
                        Succeeded = false,
                        Data = null,
                        Errors = "401 Unauthorized",
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    });
                    break;
                // 处理 403 状态码
                case StatusCodes.Status403Forbidden:
                    context.Response.ContentType = "application/json";
                    //context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsJsonAsync(new RESTfulResult<object>
                    {
                        StatusCode = StatusCodes.Status403Forbidden,
                        ErrorCode = StatusCodes.Status401Unauthorized,
                        Succeeded = false,
                        Data = null,
                        Errors = "403 Forbidden",
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    });
                    break;

                default:
                    break;
            }
        }
    }
}