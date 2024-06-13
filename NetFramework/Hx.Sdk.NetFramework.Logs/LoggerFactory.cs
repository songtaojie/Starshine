using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Logs
{
    /// <summary>
    /// 日志工厂，可以使用log4net和nlog
    /// </summary>
    public class LoggerFactory : ILoggerFactory
    {
        private LoggerType type;


        /// <summary>
        /// 使用指定的日志框架初始化对象，
        /// </summary>
        /// <param name="type">日志框架的类型，有log4net和NLog两个日志框架可供选择</param>
        public LoggerFactory(LoggerType type)
        {
            this.type = type;
        }
        /// <summary>
        /// 默认构造函数，默认使用的是log4net框架写入日志
        /// </summary>
        public LoggerFactory()
        {
            this.type = LoggerType.Log4Net;
        }
        /// <summary>
        /// 是否使用配置文件
        /// </summary>
        public bool UseConfig { get; set; } = true;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigPath { get; set; }

        /// <summary>
        /// 使用指定的日志名称创建日志对象
        /// </summary>
        /// <param name="loggerName"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public NetFramework.Logs.ILogger CreateLogger(string loggerName = null, string category = null)
        {
            NetFramework.Logs.ILogger log = null;
            if (string.IsNullOrEmpty(loggerName)) loggerName = "Default";
            if (this.type == LoggerType.Log4Net)
            {
                log = new Log4NetLogger(loggerName, category,UseConfig,ConfigPath);
            }
            else if (this.type == LoggerType.NLog)
            {
                log = new NLogLogger(loggerName, category,UseConfig,ConfigPath);
            }
            return log;
        }
    }
    /// <summary>
    /// 日志的类型，用于指定要创建给予哪个框架的服务接口
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        /// 使用Log4net框架
        /// </summary>
        Log4Net,
        /// <summary>
        /// 使用NLog框架
        /// </summary>
        NLog
    }
}
