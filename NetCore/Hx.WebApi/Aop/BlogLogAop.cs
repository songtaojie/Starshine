//using Castle.DynamicProxy;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace Hx.WebApi.Aop
//{
//    public class BlogLogAop : IInterceptor
//    {
//        private readonly IHttpContextAccessor _accessor;
//        private readonly ILogger<BlogLogAop> _logger;
//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="accessor">用户信息</param>
//        /// <param name="logger">日志记录</param>
//        public BlogLogAop(IHttpContextAccessor accessor, ILogger<BlogLogAop> logger)
//        {
//            _accessor = accessor;
//            _logger = logger;
//        }
//        /// <summary>
//        /// 实现IInterceptor唯一实例
//        /// </summary>
//        /// <param name="invocation"></param>
//        public void Intercept(IInvocation invocation)
//        {
//            var userName = _accessor.HttpContext?.User?.Identity?.Name;
//            var dataIntercept = string.Format("【当前操作用户】：{0}，\r\n【当前执行方法】：{1} ，\r\n【携带的参数有】：{2}，\r\n",
//                userName,
//                invocation.Method.Name,
//                string.Join(",", invocation.Arguments.Select(a => (a ?? "").ToString())));

//            try
//            {
//                //MiniProfiler.Current.Step($"执行Service方法：{invocation.Method.Name}() -> ");
//                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
//                invocation.Proceed();
//                if (IsAsyncMethod(invocation.Method))
//                {
//                    #region 方案一
//                    //Wait task execution and modify return value
//                    if (invocation.Method.ReturnType == typeof(Task))
//                    {
//                        invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
//                            (Task)invocation.ReturnValue,
//                            async () => await Success(invocation, dataIntercept),/*成功时执行*/
//                            ex =>
//                            {
//                                Failure(ex, dataIntercept);
//                            });
//                    }
//                    //Task<TResult>
//                    else
//                    {
//                        invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
//                         invocation.Method.ReturnType.GenericTypeArguments[0],
//                         invocation.ReturnValue,
//                          async (o) => await Success(invocation, dataIntercept, o),/*成功时执行*/
//                         ex =>
//                         {
//                             Failure(ex, dataIntercept);
//                         });
//                    }
//                    #endregion
//                }
//                else
//                {
//                    dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");
//                    Parallel.For(0, 1, e =>
//                    {
//                        _logger.LogInformation(dataIntercept);
//                    });
//                }
//            }
//            catch (Exception ex)
//            {
//                Failure(ex, dataIntercept);
//            }
//        }

//        /// <summary>
//        /// 判断该方法是否是异步方法
//        /// </summary>
//        /// <param name="method">方法</param>
//        /// <returns></returns>
//        public static bool IsAsyncMethod(MethodInfo method)
//        {
//            return
//                method.ReturnType == typeof(Task) ||
//                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
//                ;
//        }

//        private async Task Success(IInvocation invocation, string dataIntercept, object o = null)
//        {

//            dataIntercept += ($"【执行完成结果】：{JsonConvert.SerializeObject(o)}");

//            await Task.Run(() =>
//            {
//                Parallel.For(0, 1, e =>
//                {
//                    _logger.LogInformation(dataIntercept);
//                });
//            });
//        }

//        private void Failure(Exception ex, string dataIntercept)
//        {
//            if (ex != null)
//            {
//                //执行的 service 中，收录异常
//                //MiniProfiler.Current.CustomTiming("Errors：", ex.Message);
//                //执行的 service 中，捕获异常
//                dataIntercept += ($"【异常结果】：{ex.StackTrace}\r\n");

//                // 异常日志里有详细的堆栈信息
//                Parallel.For(0, 1, e =>
//                {
//                    _logger.LogInformation(dataIntercept);
//                });
//            }
//        }
//    }
//}
