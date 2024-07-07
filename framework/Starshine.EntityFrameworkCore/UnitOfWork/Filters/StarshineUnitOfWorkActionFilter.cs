using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 工作单元拦截器
    /// </summary>
    internal sealed class StarshineUnitOfWorkActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        /// <summary>
        /// 排序属性
        /// </summary>
        public int Order => 9999;

        /// <summary>
        /// 构造函数
        /// </summary>
        public StarshineUnitOfWorkActionFilter()
        {
        }

        /// <summary>
        /// 拦截请求
        /// </summary>
        /// <param name="context">动作方法上下文</param>
        /// <param name="next">中间件委托</param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }
            var actionDescriptor = context.ActionDescriptor.AsControllerActionDescriptor();
            // 获取动作方法描述器
            var method = actionDescriptor.MethodInfo;
            var unitOfWorkAttr = GetUnitOfWorkAttribute(method);
            if (unitOfWorkAttr?.IsDisabled == true)
            {
                await next();
                return;
            }
            var options = CreateOptions(context, unitOfWorkAttr);
            CancellationToken cancellationToken = context.HttpContext.RequestAborted;
            //var dbContextPool = context.HttpContext.RequestServices.GetRequiredService<IDbContextPool>();

            // 解析工作单元服务
            var unitOfWorkManager = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWorkManager>();
            using var unitOfWork = unitOfWorkManager.Begin(options);
            var resultContext = await next();
            if (Succeed(resultContext))
            {
                await unitOfWork.CompleteAsync(cancellationToken);
            }
            else
            {
                await unitOfWork.RollbackAsync(cancellationToken);
            }
        }

        private UnitOfWorkOptions CreateOptions(ActionExecutingContext context, UnitOfWorkAttribute? unitOfWorkAttribute)
        {
            var options = new UnitOfWorkOptions();

            unitOfWorkAttribute?.SetOptions(options);

            if (unitOfWorkAttribute?.IsTransactional == null)
            {
                options.IsTransactional = !string.Equals(context.HttpContext.Request.Method, HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase);
            }

            return options;
        }

        /// <summary>
        /// 判断请求是否成功
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool Succeed(ActionExecutedContext result)
        {
            return result.Exception == null || result.ExceptionHandled;
        }


        private UnitOfWorkAttribute? GetUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            if (methodInfo.DeclaringType != null)
            {
                attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
                if (attrs.Length > 0)
                {
                    return attrs[0];
                }
            }

            return null;
        }
    }
}