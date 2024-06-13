using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.Common.DependencyInjection
{
    /// <summary>
    /// 单例模式
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class SingletonDependencyAttribute : Attribute
    {
    }
}
