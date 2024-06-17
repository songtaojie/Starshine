using System.ComponentModel;

namespace Starshine.DependencyInjection
{
    /// <summary>
    /// 注册范围
    /// </summary>
    [SkipScan,Flags]
    public enum DependencyInjectionPattern
    {
        /// <summary>
        /// 第一个接口,默认值
        /// </summary>
        [Description("只注册第一个接口")]
        FirstInterface = 1,

        /// <summary>
        /// 所有接口
        /// </summary>
        [Description("所有接口")]
        AllInterfaces,

        /// <summary>
        /// 只注册自己
        /// </summary>
        [Description("只注册自己")]
        Self,
    }
}