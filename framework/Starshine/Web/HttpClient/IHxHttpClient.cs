using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Starshine
{
    /// <summary>
    /// HxHttpClient帮助类
    /// </summary>
    public interface IHxHttpClient
    {
        #region HttpGet

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="headers">请求Headers</param>
        /// <param name="resultFunc">自定义结果处理函数</param>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <returns></returns>
        Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> headers = null,
            Func<string, TResult> resultFunc = null);

        #endregion

        #region HttpPost

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="headers">请求Headers</param>
        /// <param name="contentFunc">自定义输入参数格式处理函数,默认使用Json格式处理</param>
        /// <param name="resultFunc">自定义结果处理函数</param>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <returns></returns>
        Task<TResult> PostAsync<TResult>(string url, object data, Dictionary<string, string> headers = null,
            Func<object, HttpContent> contentFunc = null, Func<string, TResult> resultFunc = null);

        #endregion
    }
}
