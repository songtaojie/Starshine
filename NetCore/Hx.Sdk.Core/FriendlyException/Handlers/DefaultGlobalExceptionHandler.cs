using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hx.Sdk.Core.FriendlyException
{
    /// <summary>
    /// 默认的异常处理
    /// </summary>
    internal class DefaultGlobalExceptionHandler : IGlobalExceptionHandler
    {
        private readonly ILogger<DefaultGlobalExceptionHandler> _logger;
        public DefaultGlobalExceptionHandler(ILogger<DefaultGlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            await Task.CompletedTask;
        }
    }
}
