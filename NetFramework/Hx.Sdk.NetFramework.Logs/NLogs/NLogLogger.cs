using System;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hx.NetFramework.Logs.NLogs;

namespace Hx.Sdk.NetFramework.Logs
{
    /// <summary>
    /// 使用NLog记录日志的方式
    /// </summary>
    internal class NLogLogger : NetFramework.Logs.ILogger
    {
        private NLog.ILogger log;
        internal bool UseConfig { private get; set; }
        internal string ConfigPath { private get; set; }
        /// <summary>
        /// 获取NLoger对象，如果没有添加配置文件，则使用默认的配置创建对象
        /// </summary>
        /// <param name="loggerName">日志名称，对应配置文件中rules->logger标签中的name属性的值</param>
        /// <param name="category">日志文件的上一层目录，一般用来分类</param>
        protected internal NLogLogger(string loggerName,string category)
        {
            log = NLogManager.GetLogger(loggerName, category);
        }
        protected internal NLogLogger(string loggerName) : this(loggerName, null)
        {
        }
        protected internal NLogLogger(string loggerName, string category, bool useConfig, string configPath)
        {
            NLogManager.UseConfig = useConfig;
            NLogManager.ConfigPath = configPath;
            this.log = NLogManager.GetLogger(loggerName, category);
        }
        public void Debug(string message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        public void Debug(string message, Exception ex)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(ex, message);
            }
        }

        public void Error(string message)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
        }

        public void Error(string message, Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(ex,message);
            }
        }

        public void Fatal(string message)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
        }

        public void Fatal(string message, Exception ex)
        {
            if(log.IsFatalEnabled)
            {
                log.Fatal(ex,message);
            }
        }

        public void Info(string message)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        public void Info(string message, Exception ex)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(ex,message);
            }
        }

        public void Warn(string message)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
        }

        public void Warn(string message, Exception ex)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(ex,message);
            }
        }

        public void Write(DateTime start, MethodInfo method, object[] parameters, object result, int resultLength)
        {
            double totalMilliseconds = (DateTime.Now - start).TotalMilliseconds;
            ParameterInfo[] infos = method.GetParameters();
            int infoLength = infos.Length;
            ParameterInfo info = null;
            if (totalMilliseconds >= 20000.0 || this.log.IsDebugEnabled)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("数据包类型: ");
                stringBuilder.Append("参数信息: ");

                for (int i = 1; i <= parameters.Length; i++)
                {
                    if (i - 1 <= infoLength - 1)
                    {
                        info = infos[i - 1];
                        stringBuilder.Append(string.Format("P{0}:类型：{1}, 值：{2}; ", i, info.ParameterType.ToString(), parameters[i - 1]));
                    }
                    else
                    {
                        stringBuilder.Append(string.Format("P{0}: {1}; ", i, parameters[i - 1]));
                    }
                }
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(string.Format("方法定义: 方法名 = {0}; 所属 = [{1}]{2}", method, method.DeclaringType.Name, method.DeclaringType.FullName));
                stringBuilder.AppendLine(string.Format("返回结果: {0};类型：{1}， 大小: {2}", result, method.ReturnType.ToString(), resultLength));
                stringBuilder.AppendLine(string.Format("开始时间: DateTime = {0:yyyy-MM-dd HH:mm:ss}.{1:000}; Duration = {2}", start, start.Millisecond, totalMilliseconds));
                stringBuilder.Append("----------------------------------------------------");
                if (totalMilliseconds >= 20000.0)
                {
                    this.Warn(stringBuilder.ToString());
                }
                else
                {
                    this.Debug(stringBuilder.ToString());
                }
            }
            if (!this.log.IsDebugEnabled && this.log.IsInfoEnabled)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(string.Format("[Invoke]: {0}.{1}; ", method.DeclaringType.Name, this.GetMethodName(method)));
                for (int i = 1; i <= parameters.Length; i++)
                {
                    if (i - 1 <= infoLength - 1)
                    {
                        info = infos[i - 1];
                        stringBuilder.Append(string.Format("P{0}:类型：{1}, 值：{2}; ", i, info.ParameterType.ToString(), parameters[i - 1]));
                    }
                    else
                    {
                        stringBuilder.Append(string.Format("[P{0}]: {1}; ", i, parameters[i - 1]));
                    }
                }
                stringBuilder.Append(string.Format("[RESULT]: {0};[Type]:{1}; [LEN]: {2}; ", result, method.ReturnType.ToString(), resultLength));
                stringBuilder.Append(string.Format("[TIME]: {0:HH:mm:ss}.{1:000}({2:0}ms)", start, start.Millisecond, totalMilliseconds));
                this.Info(stringBuilder.ToString());
            }
        }

        protected string GetMethodName(MethodInfo method)
        {
            List<string> list = new List<string>();
            foreach (ParameterInfo parameterInfo in method.GetParameters())
            {
                list.Add(parameterInfo.ParameterType.FullName);
            }
            return string.Format("{0}({1})", method.Name, string.Join(", ", list.ToArray()));
        }
    }
}
