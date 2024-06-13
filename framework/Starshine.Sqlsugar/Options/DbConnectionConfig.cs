using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.Sqlsugar
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public sealed class DbConnectionConfig : ConnectionConfig
    {
        /// <summary>
        /// 启用库表初始化
        /// </summary>
        public bool EnableInitDb { get; set; }

        /// <summary>
        /// 启用种子初始化
        /// </summary>
        public bool EnableInitSeed { get; set; }

        /// <summary>
        /// 启用驼峰转下划线
        /// </summary>
        public bool EnableUnderLine { get; set; }

        /// <summary>
        /// 启用库表差异日志
        /// </summary>
        public bool EnableDiffLog { get; set; }

        /// <summary>
        /// 启用Sql日志记录
        /// </summary>
        public bool EnableSqlLog { get; set; }

        internal ConnectionConfig ToConnectionConfig()
        {
            return new ConnectionConfig
            {
                AopEvents = AopEvents,
                ConfigId = ConfigId,
                ConfigureExternalServices = ConfigureExternalServices,
                ConnectionString = ConnectionString,
                DbLinkName = DbLinkName,
                DbType = DbType,
                IndexSuffix = IndexSuffix,
                IsAutoCloseConnection = IsAutoCloseConnection,
                LanguageType = LanguageType,
                MoreSettings = MoreSettings,
                SlaveConnectionConfigs = SlaveConnectionConfigs,
                SqlMiddle = SqlMiddle
            };
        }
    }
}
