using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Sqlsugar
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public class DbSettingsOptions : IConfigureOptions<DbSettingsOptions>
    {
        /// <summary>
        /// 数据库连接配置
        /// </summary>
        public IEnumerable<DbConnectionConfig>? ConnectionConfigs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public void Configure(DbSettingsOptions options)
        {
            options.ConnectionConfigs ??= new List<DbConnectionConfig>();
            foreach (var dbConfig in options.ConnectionConfigs)
            {
                if (dbConfig.ConfigId == null || string.IsNullOrWhiteSpace(dbConfig.ConfigId.ToString()))
                    dbConfig.ConfigId = SqlSugarConfigProvider.DefaultConfigId;
            }
        }
    }
}
