using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Logs
{
    /// <summary>
    /// 使用log4net记录日志信息
    /// </summary>
    internal class Log4NetLogger : NetFramework.Logs.ILogger
    {
        /// <summary>
        /// log4net对象
        /// </summary>
        private log4net.ILog log;

        internal bool UseConfig { private get; set; }
        internal string ConfigPath { private get; set; }
        /// <summary>
        /// 使用loggerName创建对象
        /// </summary>
        /// <param name="loggerName"></param>
        protected internal Log4NetLogger(string loggerName):this(loggerName,null)
        {
        }
        protected internal Log4NetLogger(string loggerName,string category)
        {
            this.log = Log4NetManager.GetLogger(loggerName, category);
        }
        protected internal Log4NetLogger(string loggerName, string category,bool useConfig, string configPath)
        {
            Log4NetManager.UseConfig = useConfig;
            Log4NetManager.ConfigPath = configPath;
            this.log = Log4NetManager.GetLogger(loggerName, category);
        }
        public void Debug(string message)
        {
            if (this.log.IsDebugEnabled)
            {
                this.log.Debug(message);
            }
        }

        public void Debug(string message, Exception ex)
        {
            if (this.log.IsDebugEnabled)
            {
                this.log.Debug(message,ex);
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
                log.Error(message,ex);
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
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
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
                log.Info(message,ex);
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
                log.Warn(message,ex);
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
                stringBuilder.AppendLine(string.Format("返回结果: {0};类型：{1}， 大小: {2}",result, method.ReturnType.ToString(), resultLength));
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
