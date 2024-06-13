using Starshine.FriendlyException;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Microsoft.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 友好异常拦截器
    /// </summary>
    [SkipScan]
    public sealed class FriendlyExceptionFilter : IAsyncExceptionFilter
    {
        /// <summary>
        /// 服务提供器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务提供器</param>
        public FriendlyExceptionFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 异步异常的处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            //解析异常处理服务，实现自定义异常额外操作，如记录日志等
            var globalExceptionHandler = _serviceProvider.GetService<IGlobalExceptionHandler>();
            if (globalExceptionHandler != null)
            {
                await globalExceptionHandler.OnExceptionAsync(context);
            }

            // 排除 WebSocket 请求处理
            if (context.HttpContext.IsWebSocketRequest()) return;

            // 如果异常在其他地方被标记了处理，那么这里不再处理
            if (context.ExceptionHandled) return;

            // 解析异常信息
            var exceptionMetadata = UnifyResultContext.GetExceptionMetadata(context);

            // 判断是否是 Razor Pages
            var isPageDescriptor = context.ActionDescriptor is CompiledPageActionDescriptor;
            // 设置异常结果
            var exception = context.Exception;
            // 解析验证异常
            var validationFlag = "[Validation]";
            var isValidationException = exception.Message.StartsWith(validationFlag);
            var errorMessage = isValidationException ? exception.Message[validationFlag.Length..] : exception.Message;

            if (isPageDescriptor)
            {
                // 返回自定义错误页面
                context.Result = new BadPageResult(isValidationException ? StatusCodes.Status400BadRequest : exceptionMetadata.StatusCode)
                {
                    Title = isValidationException ? "ModelState Invalid" : $"Internal Server: {errorMessage}",
                    Code = context.Exception.ToString()
                };
            }
            else // Mvc / WebApi
            {

                // 排除 Mvc 控制器处理
                var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                if (typeof(Controller).IsAssignableFrom(actionDescriptor.ControllerTypeInfo)) return;
                // 判断是否跳过规范化结果
                if (UnifyResultContext.IsSkipUnifyHandler(context, out var unifyResult))
                {
                    // 解析异常信息
                    context.Result = new ContentResult
                    {
                        Content = exceptionMetadata.ErrorMessage,
                        StatusCode = exceptionMetadata.StatusCode
                    };
                }
                else context.Result = unifyResult.OnException(context, exceptionMetadata);

            }
            await Task.CompletedTask;
        }
    }
}
