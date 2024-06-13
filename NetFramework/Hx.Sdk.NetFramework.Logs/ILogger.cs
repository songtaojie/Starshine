using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Logs
{
    /// <summary>
    /// 写入日志的接口，实现可以使用log4net、NLog
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 调试程序的记录信息
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);
        /// <summary>
        /// 调试程序的记录信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Debug(string message, Exception ex);
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Error(string message, Exception ex);
        /// <summary>
        /// 致命的错误信息
        /// </summary>
        /// <param name="message"></param>
        void Fatal(string message);
        /// <summary>
        /// 致命的信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Fatal(string message, Exception ex);
        /// <summary>
        /// 信息类型的信息
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);
        /// <summary>
        /// 信息类型的信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Info(string message, Exception ex);
        /// <summary>
        /// 警告类型的额信息
        /// </summary>
        /// <param name="message"></param>
        void Warn(string message);
        /// <summary>
        /// 警告类型的信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Warn(string message, Exception ex);
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="start">开始的时间</param>
        /// <param name="method">方法的元数据</param>
        /// <param name="parameters">当前方法的参数值</param>
        /// <param name="result">当前方法的返回结果</param>
        /// <param name="resultLength">返回结果的长度</param>
        void Write(DateTime start, MethodInfo method, object[] parameters, object result, int resultLength);
    }
}
