using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Cache
{
    /// <summary>
    /// Memcached的帮助类，需要在web.config或者app.config中配置
    /// </summary>
    /// <include file='Enyim.xml' path='Enyim.xml'/>
    public sealed class MemcachedHelper
    {
        private static MemcachedClient mc = null;

        static readonly object padlock = new object();
        //线程安全的单例模式
        internal static MemcachedClient Instance
        {
            get
            {
                if (mc == null)
                {
                    lock (padlock)
                    {
                        if (mc == null) mc = new MemcachedClient();
                    }
                }
                return mc;
            }
        }

        static MemcachedClient MemClientInit()
        {
            //初始化缓存
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
            //           IPAddress newaddress =
            //  IPAddress.Parse(Dns.GetHostEntry
            //("your_ocs_host").AddressList[0].ToString())；//your_ocs_host替换为OCS内网地址
            //           IPEndPoint ipEndPoint = new IPEndPoint(newaddress, 11211);
            //           // 配置文件 - ip
            //           memConfig.Servers.Add(ipEndPoint);
            string[] serverlist = Config.ConfigManager.MemcachedServices;
            foreach (var address in serverlist)
            {
                memConfig.AddServer(address);
            }
            // 配置文件 - 协议
            memConfig.Protocol = MemcachedProtocol.Binary;
            // 配置文件-权限
            memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
            memConfig.Authentication.Parameters["zone"] = "";
            //memConfig.Authentication.Parameters["userName"] = "username";
            //memConfig.Authentication.Parameters["password"] = "password";
            //下面请根据实例的最大连接数进行设置
            memConfig.SocketPool.MinPoolSize = 5;
            memConfig.SocketPool.MaxPoolSize = 200;
            mc = new MemcachedClient(memConfig);
            return mc;
        }
        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool Set(string key, object value)
        {
            var data = Instance.Get(key);
            if (data == null)
            {
                return mc.Store(StoreMode.Add, key, value);
            }
            else
            {
                return mc.Store(StoreMode.Replace, key, value);
            }
        }
        /// <summary>
        /// 存储数据，并设置过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="time">什么时间过期</param>
        /// <returns></returns>
        public static bool Set(string key, object value, DateTime time)
        {
            var data = Instance.Get(key);
            if (data == null)
            {
                return mc.Store(StoreMode.Add, key, value, time);
            }
            else
            {
                return mc.Store(StoreMode.Replace, key, value, time );
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return Instance.Get(key);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(string key)
        {
            return Instance.Remove(key);
        }

        /// <summary>
        /// 清空缓存服务器上的缓存
        /// </summary>
        public static void Clear()
        {
            Instance.FlushAll();
        }
    }
}
