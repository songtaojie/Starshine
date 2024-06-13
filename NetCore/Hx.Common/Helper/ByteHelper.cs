using Hx.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Common
{
    /// <summary>
    /// 字节帮助类
    /// </summary>
    public static class ByteHelper
    {
        /// <summary>
        /// 把字节数组转换为对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object ToObject(byte[] bytes)
        {
            if (bytes != null && bytes.Length != 0)
            {
                try
                {
                    return new Serializer().BinaryDeserialize<object>(bytes);
                }
                catch (Exception)
                {
                    byte[] decBytes = new Compressor().Decompress(bytes);
                    return new Serializer().BinaryDeserialize<object>(decBytes);
                }
            }
            return null;
        }
        /// <summary>
        /// 把字节数组转换成指定的对象
        /// </summary>
        /// <typeparam name="TInfo"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static TInfo ToObject<TInfo>(byte[] bytes)
        {
            if (bytes != null && bytes.Length != 0)
            {
                try
                {
                    return new Serializer().BinaryDeserialize<TInfo>(bytes);
                }
                catch (Exception)
                {
                    byte[] decBytes = new Compressor().Decompress(bytes);
                    return new Serializer().BinaryDeserialize<TInfo>(decBytes);
                }
            }
            return default(TInfo);
        }
        /// <summary>
        /// 把对象转换成字节数组
        /// </summary>
        /// <param name="objInfo"></param>
        /// <returns></returns>
        public static byte[] ToBytes(object objInfo)
        {
            if (objInfo != null)
            {
                return new Serializer().BinarySerialize<object>(objInfo);
            }
            return new byte[0];
        }
    }
}
