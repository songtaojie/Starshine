using System.ComponentModel;

namespace Hx.Common.DependencyInjection
{
    /// <summary>
    /// 注册范围
    /// </summary>
    [SkipScan]
    public enum InjectionPatterns
    {
        /// <summary>
        /// 只注册自己
        /// </summary>
        [Description("只注册自己")]
        Self,

        /// <summary>
        /// 第一个接口,默认值
        /// </summary>
        [Description("只注册第一个接口")]
        FirstInterface,

        /// <summary>
        /// 所有接口
        /// </summary>
        [Description("所有接口")]
        ImplementedInterfaces,
    }
}