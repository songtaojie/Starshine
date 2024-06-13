using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.NetFramework.Cache
{
	/// <summary>
	/// redis连接的帮助类
	/// </summary>
	internal class RedisConnectionHelp
	{

		/// <summary>
		/// 连接缓存集合
		/// </summary>
		private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();

		/// <summary>
		/// 获取连接的信息
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
			if (!ConnectionCache.ContainsKey(connectionString))
			{
				ConnectionCache[connectionString] = GetManager(connectionString);
			}
			return ConnectionCache[connectionString];
		}

		/// <summary>
		/// 获取redis操作的实例
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		private static ConnectionMultiplexer GetManager(string connectionString = null)
		{
			if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
			ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString, null);
			connectionMultiplexer.ConnectionFailed += MuxerConnectionFailed;
			connectionMultiplexer.ConnectionRestored += MuxerConnectionRestored;
			connectionMultiplexer.ErrorMessage += MuxerErrorMessage;
			connectionMultiplexer.ConfigurationChanged += MuxerConfigurationChanged;
			connectionMultiplexer.HashSlotMoved += MuxerHashSlotMoved;
			connectionMultiplexer.InternalError += MuxerInternalError;
			return connectionMultiplexer;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
		{
			Console.WriteLine("Configuration changed: " + e.EndPoint);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
		{
			Console.WriteLine("ErrorMessage: " + e.Message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
		{
			Console.WriteLine("ConnectionRestored: " + e.EndPoint);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
		{
			Console.WriteLine(string.Concat(new object[]
			{
				"重新连接：Endpoint failed: ",
				e.EndPoint,
				", ",
				e.FailureType,
				(e.Exception == null) ? "" : (", " + e.Exception.Message)
			}));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
		{
			Console.WriteLine(string.Concat(new object[]
			{
				"HashSlotMoved:NewEndPoint",
				e.NewEndPoint,
				", OldEndPoint",
				e.OldEndPoint
			}));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
		{
			Console.WriteLine("InternalError:Message" + e.Exception.Message);
		}
	}
}
