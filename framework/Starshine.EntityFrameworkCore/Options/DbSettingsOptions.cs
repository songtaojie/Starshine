﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// DbSettings
    /// </summary>
    public class DbSettingsOptions : IPostConfigureOptions<DbSettingsOptions>
    {
        /// <summary>
        /// 集成 MiniProfiler 组件
        /// </summary>
        public bool? EnabledMiniProfiler { get; set; }

        /// <summary>
        /// 是否打印连接信息
        /// </summary>
        public bool? PrintConnectionString { get; set; }

        /// <summary>
        /// 是否开启日志记录
        /// </summary>
        public bool? EnabledSqlLog { get; set; }

       
        /// <summary>
        /// 后置配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void PostConfigure(string name, DbSettingsOptions options)
        {
            options.EnabledMiniProfiler ??= true;
            options.PrintConnectionString ??= true;
            options.EnabledSqlLog ??= true;
        }
    }
}
