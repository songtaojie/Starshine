using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Logs
{
    /// <summary>
    /// 日志工厂的接口
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// 使用指定的日志名称创建日志对象，如果没有指定名称使用默认的名称，并使用默认的配置来注册
        /// </summary>
        /// <param name="loggerName">日志名称,
        /// <para>对应NLog配置文件中&lt;rules> &lt;logger name="loggerName"> &lt;logger>&lt;/rules> 标签中的name属性的值;</para>
        /// <para>对应log4net配置文件标签&lt;logger name="loggerName"&gt;中的name的值</para>
        /// </param>
        /// <param name="category">日志文件的上一层目录，一般用来分类</param>
        /// <returns></returns>
        NetFramework.Logs.ILogger CreateLogger(string loggerName = null, string category = null);

        /// <summary>
        /// 是否使用配置文件，默认是false
        /// <para>如果为false，代表不使用配置文件，则使用系统内置的配置来创建日志对象，</para>
        /// <para>如果设置为true，设置属性<seealso cref="ConfigPath"/>的值指定配置文件的路径</para>
        /// </summary>
        bool UseConfig { get; set; }

        /// <summary>
        /// 配置文件的路径，没有指定时默认是加载App.config/web.config
        /// </summary>
        string ConfigPath { get; set; }
    }
}
