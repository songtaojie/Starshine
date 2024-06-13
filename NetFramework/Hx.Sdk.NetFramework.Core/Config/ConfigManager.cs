using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Config
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// 获取Memcached的服务器端口的配置,多个服务器使用,分割
        /// </summary>
        public static string[] MemcachedServices
        {
            get
            {
                string service = ConfigurationManager.AppSettings["MemcachedServices"];
                if (string.IsNullOrEmpty(service))
                {
                    return new string[] { "127.0.0.1:11211" };
                }
                else
                {
                    return service.Split(',');
                }
            }
        }
        /// <summary>
        /// 获取Web.Config中的AppSetting中节点的值
        /// </summary>
        /// <param name="key">AppSetting中节点键</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static string GetAppSettingValue(string key, string defValue = null)
        {
            if (string.IsNullOrEmpty(key)) return defValue;
            return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 获取Web.Config中的AppSetting中节点的值
        /// </summary>
        /// <param name="key">AppSetting中节点键</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static long GetAppSettingValue(string key, long defValue)
        {
            if (string.IsNullOrEmpty(key)) return defValue;
            string value = ConfigurationManager.AppSettings[key];
            long.TryParse(value, out defValue);
            return defValue;
        }
    }
}
