using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.Common.DependencyInjection
{
    /// <summary>
    /// 每个请求一个实例
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class ScopedDependencyAttribute : Attribute
    {
    }
}
