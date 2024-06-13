using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace Starshine
{
    /// <summary>
    /// HxHttpClient实现类
    /// </summary>
    internal class HxHttpClient: IHxHttpClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<HxHttpClient> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public HxHttpClient(HttpClient client, ILogger<HxHttpClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        #region HttpGet

        /// <inheritdoc cref="IHxHttpClient.GetAsync{TResult}"/>
        public async Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> headers = null,
            Func<string, TResult> resultFunc = null)
        {
            return await CallRequestAsync(async httpClient => await httpClient.GetAsync(url), headers, resultFunc);
        }

        #endregion

        #region HttpPost

        /// <inheritdoc cref="IHxHttpClient.PostAsync{TResult}"/>
        public async Task<TResult> PostAsync<TResult>(string url, object data,
            Dictionary<string, string> headers = null,
            Func<object, HttpContent> contentFunc = null, Func<string, TResult> resultFunc = null)
        {
            return await CallRequestAsync(async httpClient =>
            {
                HttpContent content;
                if (contentFunc != null)
                {
                    content = contentFunc.Invoke(data);
                }
                else
                {
                    var jsonData = JsonSerializer.Serialize(data);
                    content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }

                return await httpClient.PostAsync(url, content);
            }, headers, resultFunc);
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="headers"></param>
        /// <param name="resultFunc"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        private async Task<TResult> CallRequestAsync<TResult>(
            Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> operation,
            Dictionary<string, string> headers = null, Func<string, TResult> resultFunc = null)
        {
            _client.DefaultRequestHeaders.Clear();
            if (headers != null && headers.Count > 0)
            {
                foreach (var (key, value) in headers)
                {
                    _client.DefaultRequestHeaders.Add(key, value);
                }
            }

            var response = await operation(_client);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"request failed, response code:{response.StatusCode},msg:{await response.Content.ReadAsStringAsync()}");
                return default;
            }

            var result = await response.Content.ReadAsStringAsync();
            try
            {
                return resultFunc != null ? resultFunc(result) : JsonSerializer.Deserialize<TResult>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"deserialize failed,content:{result},Msg:{ex.StackTrace},Url:{response.RequestMessage.RequestUri}");
                return default;
            }
        }

        #endregion
    }
}
