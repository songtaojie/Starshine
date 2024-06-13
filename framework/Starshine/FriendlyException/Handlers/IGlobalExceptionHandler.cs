using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Starshine.FriendlyException
{
    /// <summary>
    /// 全局异常处理接口
    /// </summary>
    public interface IGlobalExceptionHandler
    {
        /// <summary>
        /// 异常拦截
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnExceptionAsync(ExceptionContext context);
    }
}