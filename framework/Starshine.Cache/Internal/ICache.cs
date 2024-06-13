using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 缓存的实例对象
        /// </summary>
        object Instance { get; }

        /// <summary>
        /// 缓存实例类型
        /// </summary>
        CacheTypeEnum CacheType { get; }
        /// <summary>
        /// 获取所有的缓存的key
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllKeys();
        /// <summary>
        /// 获取和设置缓存，永不过期
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>
        ///  缓存个数
        /// </summary>
        long Count { get; }

        /// <summary>
        /// 是否包含缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ExistsKey(string key);

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间，秒，-1：表示不过期</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, int expire);

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <typeparam name="T">数据的类型参数</typeparam>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
		/// 获取数据
		/// </summary>
		/// <param name="key">缓存的键</param>
		/// <returns></returns>
		string Get(string key);

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Set(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// 批量移除缓存项
        /// </summary>
        /// <param name="keys">键集合</param>
        void Remove(params string[] keys);

        /// <summary>
        /// 根据前缀移除缓存项
        /// </summary>
        /// <param name="prefixKey"></param>
        /// <returns></returns>
        long RemoveByPrefix(string prefixKey);

        /// <summary>
        ///  清空所有缓存项
        /// </summary>
        void Clear();

        /// <summary>
        /// 设置Redis数据库
        /// </summary>
        /// <param name="dbNum"></param>
        void SetRedisDbNum(int dbNum);

        /// <summary>
        ///  获取指定键的数据结构类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        KeyType TYPE(string key);
    }
}
