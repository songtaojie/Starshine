using Starshine.FriendlyException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Starshine.Common.Extensions;

namespace Starshine
{
    /// <summary>
    /// 规范化结果上下文
    /// </summary>
    [SkipScan]
    public static class UnifyResultContext
    {
        /// <summary>
        /// 是否启用规范化结果
        /// </summary>
        internal static bool IsEnabledUnifyHandle = false;

        /// <summary>
        /// 规范化结果类型
        /// </summary>
        internal static Type? RESTfulResultType = typeof(RESTfulResult<>);

        /// <summary>
        /// 规范化结果额外数据键
        /// </summary>
        internal static string UnifyResultExtrasKey = "UNIFY_RESULT_EXTRAS";

        /// <summary>
        /// 规范化结果状态码
        /// </summary>
        internal static string UnifyResultStatusCodeKey = "UNIFY_RESULT_STATUS_CODE";

        /// <summary>
        /// 获取异常元数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ExceptionMetadata GetExceptionMetadata(ActionContext context)
        {
            // 获取错误码
            object? errorCode = default;
            object? errors = default;
            string? errorMessage = null;
            object? data = default;
            var statusCode = StatusCodes.Status500InternalServerError;
            // 判断是否是 ExceptionContext 或者 ActionExecutedContext
            var exception = context is ExceptionContext exContext
                ? exContext.Exception
                : (
                    context is ActionExecutedContext edContext
                    ? edContext.Exception
                    : default
                );
            if (exception is UserFriendlyException friendlyException)
            {
                errorCode = friendlyException.ErrorCode;
                statusCode = friendlyException.StatusCode;
                errors = friendlyException.ErrorMessage;
                data = friendlyException.Data;
                errorMessage = friendlyException.ErrorMessage;
            }
            else if (exception != null)
            {
                errorMessage = exception.Message;
                errors = exception.Message;
            }
            var validationFlag = "[Validation]";

            // 处理验证失败异常
            if (!string.IsNullOrEmpty(errorMessage) && errorMessage.StartsWith(validationFlag))
            {
                // 处理结果
                errorMessage = errorMessage[validationFlag.Length..];
                // 设置为400状态码
                statusCode = StatusCodes.Status400BadRequest;
            }

            return new ExceptionMetadata
            {
                Data = data,
                ErrorCode = errorCode,
                Errors = errors,
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            };
        }

        /// <summary>
        /// 是否跳过成功返回结果规范处理（状态码 200~209 ）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unifyResultProvider"></param>
        /// <param name="isWebRequest"></param>
        /// <returns></returns>
        internal static bool IsSkipSucceedUnifyHandler(ActionContext context, out IUnifyResultProvider? unifyResultProvider, bool isWebRequest = true)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor == null)
            {
                unifyResultProvider = null;
                return false;
            }
            var method = actionDescriptor.MethodInfo;
            // 判断是否跳过规范化处理
            var isSkip = !IsEnabledUnifyHandle
                  || method.GetRealReturnType().HasImplementedRawGeneric(RESTfulResultType)
                  || method.CustomAttributes.Any(x => typeof(NonUnifyAttribute).IsAssignableFrom(x.AttributeType) || typeof(ProducesResponseTypeAttribute).IsAssignableFrom(x.AttributeType) || typeof(IApiResponseMetadataProvider).IsAssignableFrom(x.AttributeType))
                  || (method.ReflectedType != null && method.ReflectedType.IsDefined(typeof(NonUnifyAttribute), true));

            if (!isWebRequest)
            {
                unifyResultProvider = null;
                return isSkip;
            }

            unifyResultProvider = isSkip ? null : context.HttpContext.RequestServices.GetService<IUnifyResultProvider>();
            return unifyResultProvider == null || isSkip;
        }

        /// <summary>
        /// 是否跳过规范化处理（包括任意状态：成功，失败或其他状态码）
        /// </summary>
        /// 
        /// <param name="context"></param>
        /// <param name="unifyResult"></param>
        /// <param name="isWebRequest"></param>
        /// <returns></returns>
        internal static bool IsSkipUnifyHandler(ActionContext context, out IUnifyResultProvider? unifyResult, bool isWebRequest = true)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor == null)
            {
                unifyResult = null;
                return false;
            }
            var method = actionDescriptor.MethodInfo;
            // 判断是否跳过规范化处理
            var isSkip = !IsEnabledUnifyHandle
                    || method.CustomAttributes.Any(x => typeof(NonUnifyAttribute).IsAssignableFrom(x.AttributeType))
                    || (
                            !method.CustomAttributes.Any(x => typeof(ProducesResponseTypeAttribute).IsAssignableFrom(x.AttributeType))
                            && (method.ReflectedType != null && method.ReflectedType.IsDefined(typeof(NonUnifyAttribute), true))
                        );

            if (!isWebRequest)
            {
                unifyResult = null;
                return isSkip;
            }

            unifyResult = isSkip ? null : context.HttpContext.RequestServices.GetService<IUnifyResultProvider>();
            return unifyResult == null || isSkip;
        }

        /// <summary>
        /// 是否跳过特定状态码规范化处理（如，处理 401，403 状态码情况）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unifyResult"></param>
        /// <returns></returns>
        internal static bool IsSkipUnifyHandler(HttpContext context, out IUnifyResultProvider? unifyResult)
        {
            // 获取终点路由特性
            var endpointFeature = context.Features.Get<IEndpointFeature>();
            if (endpointFeature == null) return (unifyResult = null) == null;

            // 判断是否跳过规范化处理
            var isSkip = !IsEnabledUnifyHandle || context.GetMetadata<NonUnifyAttribute>() != null;

            unifyResult = isSkip ? null : context.RequestServices.GetService<IUnifyResultProvider>();
            return unifyResult == null || isSkip;
        }

        /// <summary>
        /// 获取 Action 特性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        internal static TAttribute? GetMetadata<TAttribute>(this HttpContext httpContext)
            where TAttribute : class
        {
            return httpContext.GetEndpoint()?.Metadata?.GetMetadata<TAttribute>();
        }
    }
}