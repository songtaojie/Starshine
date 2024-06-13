using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.NetFramework.Logs.NLogs
{
    internal static class NLogManager
    {
        private static readonly ConcurrentDictionary<string, NLog.ILogger> loggerContainer = new ConcurrentDictionary<string, NLog.ILogger>();
        internal static bool UseConfig
        {
            private get; set;
        } = false;
        internal static string ConfigPath
        {
            private get; set;
        }

        /// <summary>
        /// 获取NLoger对象，如果没有添加配置文件，则使用默认的配置创建对象
        /// </summary>
        /// <param name="loggerName">日志名称，对应配置文件中rules-logger标签中的name属性的值</param>
        /// <param name="category">日志文件的上一层目录，一般用来分类</param>
        /// <returns></returns>
        internal static NLog.ILogger GetLogger(string loggerName, string category = null)
        {
            if (loggerContainer.ContainsKey(loggerName)) return loggerContainer[loggerName];
            LoggingConfiguration config = LogManager.Configuration;
            //加载配置
            if (UseConfig && (config ==null || config.AllTargets.Count == 0))
            {
                string path = ConfigPath;
                //首先判断根目录下是否有log4net.config文件
                if (string.IsNullOrEmpty(path))
                    path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + (@"\nlog.config");
                if (File.Exists(path))
                {
                    config = LogManager.LoadConfiguration(path).Configuration;
                }
            }
            if (UseConfig && !Exists(loggerName))
                throw new Exception("获取对象失败,请指定正确的日志名或者配置文件!");
            if (config == null)
            {
                config = new LoggingConfiguration();
                //控制台 创建Target，并添加成员变量
                ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget()
                {
                    Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}"
                };
                config.AddTarget("console", consoleTarget);
                //文件输出
                string cg = string.IsNullOrEmpty(category) ? "" : category+"/";
                FileTarget fileTarget = new FileTarget()
                {
                    FileName = "${basedir}/Log/" + cg + "${shortdate}.log",
                    Layout = "${longdate} | ${message} ${onexception:${exception:format=tostring} ${newline} ${stacktrace} ${newline}"
                };
                config.AddTarget("file", fileTarget);
                // 定义规则
                LoggingRule rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                config.LoggingRules.Add(rule1);
                LoggingRule rule2 = new LoggingRule("*", LogLevel.Info, fileTarget);
                config.LoggingRules.Add(rule2);

                // 设置配置
                LogManager.Configuration = config;
            }
            Logger logger = LogManager.GetLogger(loggerName);
            loggerContainer.TryAdd(loggerName, logger);
            return logger;
        }
        /// <summary>
        /// 判断是否存在日志
        /// </summary>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        private static bool Exists(string loggerName)
        {
            LoggingConfiguration config = LogManager.Configuration;
            if (config == null || config.AllTargets.Count == 0 || config.LoggingRules.Count == 0) return false;
            bool exist = false;
            foreach (LoggingRule rule in config.LoggingRules)
            {
                if (rule.NameMatches(loggerName)) {
                    exist = true;
                    break;
                }
            }
            return exist;
        }
    }
}
