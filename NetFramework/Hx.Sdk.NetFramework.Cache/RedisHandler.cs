using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Cache
{
	/// <summary>
	/// redis的处理类
	/// 需要在config的AppSetting中配置键为RedisConnection的连接字符串
	/// </summary>
	public class RedisHandler : IRedisHandler
	{

		private readonly ConnectionMultiplexer _conn;
		private readonly IDatabase _database;
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="connectionString">redis数据库连接的字符串</param>
		public RedisHandler(string connectionString)
		{
			this._conn = RedisConnectionHelp.GetConnectionMultiplexer(connectionString);
			_database = _conn.GetDatabase();
		}

		/// <summary>
		/// key的前缀
		/// </summary>
		public string KeyPrefix;

		/// <summary>
		/// 数据库编号
		/// </summary>
		private int DbNum { get; set; }

		/// <summary>
		/// 设置数据库编号
		/// </summary>
		/// <param name="dbNum"></param>
		public void SetRedisDbNum(int dbNum = 0)
		{
			this.DbNum = dbNum;
		}

		/// <summary>
		/// 获取指定的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Get<T>(string key)
		{
			key = this.AddSysCustomKey(key);
			return this.Do<T>((IDatabase db) => JsonConvert.DeserializeObject<T>(db.StringGet(key, CommandFlags.None)));
		}

		/// <summary>
		/// 设置值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="expiry"></param>
		/// <returns></returns>
		public bool Set<T>(string key, T value, TimeSpan? expiry = null)
		{
			key = this.AddSysCustomKey(key);
			string json = JsonConvert.SerializeObject(value);
			return this.Do<bool>((IDatabase db) => db.StringSet(key, json, expiry, When.Always, CommandFlags.None));
		}

		/// <summary>
		/// 移除值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(string key)
		{
			key = this.AddSysCustomKey(key);
			return this.Do<bool>((IDatabase db) => db.KeyDelete(key, CommandFlags.None));
		}

		/// <summary>
		/// 清除某个数据库中的数据
		/// </summary>
		/// <param name="dbNum"></param>
		public void RemoveDb(int dbNum)
		{
			if (dbNum < 0 && dbNum > 15) throw new Exception("redis数据库编号范围在0-15之间");
			if (string.IsNullOrEmpty(this._conn.Configuration)) throw new Exception("数据库连接不能为空");
			this._conn.GetServer(this._conn.Configuration.Split(new char[]
			{
				','
			})[0], null).FlushDatabase(dbNum, CommandFlags.None);
		}

		/// <summary>
		/// 清除数据库中所有值
		/// </summary>
		public void Clear()
		{
			//this._conn.GetServer("", 111, null).FlushDatabase(DbNum, CommandFlags.None);
			var endpoint = _conn.GetEndPoints();
			var server = _conn.GetServer(endpoint.First());
			server.FlushAllDatabases(CommandFlags.None);
		}

		/// <summary>
		/// 异步获取值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<T> GetAsync<T>(string key)
		{
			key = this.AddSysCustomKey(key);
			return JsonConvert.DeserializeObject<T>(await this.Do<Task<RedisValue>>((IDatabase db) => db.StringGetAsync(key, CommandFlags.None)));
		}

		/// <summary>
		/// 设置值，异步
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="expiry"></param>
		/// <returns></returns>
		public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
		{
			key = this.AddSysCustomKey(key);
			string json = JsonConvert.SerializeObject(value);
			return await this.Do<Task<bool>>((IDatabase db) => db.StringSetAsync(key, json, expiry, When.Always, CommandFlags.None));
		}

		/// <summary>
		/// 移除值，异步
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<bool> RemoveAsync(string key)
		{
			key = this.AddSysCustomKey(key);
			return await this.Do<Task<bool>>((IDatabase db) => db.KeyDeleteAsync(key, CommandFlags.None));
		}

		/// <summary>
		/// 清除某个数据库中的数据
		/// </summary>
		/// <param name="dbNum"></param>
		public async Task RemoveDbAsync(int dbNum)
		{
			if (dbNum < 0 && dbNum > 15) throw new Exception("redis数据库编号范围在0-15之间");
			if (string.IsNullOrEmpty(this._conn.Configuration)) throw new Exception("数据库连接不能为空");
			await this._conn.GetServer(this._conn.Configuration.Split(new char[]
			{
				','
			})[0], null).FlushDatabaseAsync(dbNum, CommandFlags.None);
		}

		/// <summary>
		/// 清除数据库中所有值
		/// </summary>
		/// <returns></returns>
		public async Task ClearAsync()
		{
			var endpoint = _conn.GetEndPoints();
			var server = _conn.GetServer(endpoint.First());
			await server.FlushAllDatabasesAsync(CommandFlags.None);
		}

		/// <summary>
		/// 获取key
		/// </summary>
		/// <param name="oldKey"></param>
		/// <returns></returns>
		private string AddSysCustomKey(string oldKey)
		{
			return this.KeyPrefix + oldKey;
		}

		/// <summary>
		/// 获取数据库
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		private T Do<T>(Func<IDatabase, T> func)
		{
			IDatabase database = this._conn.GetDatabase(this.DbNum, null);
			return func(database);
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			this._conn.Dispose();
		}

	}
}
