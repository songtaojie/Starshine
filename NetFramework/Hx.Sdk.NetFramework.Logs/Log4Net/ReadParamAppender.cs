using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using log4net.Appender;

namespace Hx.Sdk.NetFramework.Logs
{
    /// <summary>
    /// log4net自定义添加器，并在初始化时设置AppendToFile=true;
    /// RollingStyle = Composite;
    /// StaticLogFileName = true;
    /// LockingModel = MinimalLock;
    /// </summary>
    [Obsolete]
    internal class ReadParamAppender : RollingFileAppender
    {

        public ReadParamAppender()
        {
            //this.AppendToFile = true;
            //this.RollingStyle = RollingFileAppender.RollingMode.Date;
            //this.StaticLogFileName = false;
            //this.LockingModel = new MinimalLock();
        }

        private string _layoutPattern;
        /// <summary>
        /// 日志输出格式
        /// </summary>
        public string LayoutPattern
        {
            get { return this._layoutPattern; }
            set { _layoutPattern = value; }
        }
        private string _level;
        /// <summary>
        /// logger输出等级
        /// </summary>
        public string Level
        {
            get { return this._level; }
            set { _level = value; }
        }
    }
}
