using System;
using System.IO;
using System.IO.Compression;

namespace Starshine.Common.Serialization
{
    /// <summary>
    /// 压缩类，用来压缩流数据
    /// </summary>
    public class Compressor
    {
        private int overSize;
        /// <summary>
        /// 初始化压缩类，默认overSize是0x3e8
        /// </summary>
        public Compressor()
        {
            overSize = 0x3e8;
        }
        /// <summary>
        /// 初始化压缩类，并设置最大的流的大小，默认是0x3e8
        /// </summary>
        /// <param name="oversize"></param>
        public Compressor(int oversize)
        {
            this.overSize = 0x3e8;
            this.overSize = oversize;
        }
        /// <summary>
        /// 如果传进来的字节数组的长度大于设置的overSize，则对传进来的基础流进行压缩
        /// </summary>
        /// <param name="val">要压缩的流</param>
        /// <returns>压缩后的流</returns>
        public byte[] Compress(byte[] val)
        {
            byte[] buffer;
            if ((this.overSize != -1) && (val.Length > this.overSize))
            {
                MemoryStream stream = new MemoryStream();
                using (DeflateStream stream2 = new DeflateStream(stream, CompressionMode.Compress, true))
                {
                    stream2.Write(val, 0, val.Length);
                }
                buffer = new byte[stream.Length + 1L];
                stream.ToArray().CopyTo(buffer, 1);
                stream.Close();
                buffer[0] = 1;
                return buffer;
            }
            buffer = new byte[val.Length + 1];
            val.CopyTo(buffer, 1);
            buffer[0] = 0;
            return buffer;
        }
        /// <summary>
        /// 对传进来的流进行解压缩
        /// </summary>
        /// <param name="val">需要解压缩的流</param>
        /// <returns>解压缩后的流</returns>
        public byte[] Decompress(byte[] val)
        {
            if (val[0] == 1)
            {
                DeflateStream stream = new DeflateStream(new MemoryStream(this.UnwrapData(val)), CompressionMode.Decompress);
                return ArrayHelper.ReadAllBytesFromStream(stream);
            }
            return this.UnwrapData(val);
        }
        private byte[] UnwrapData(byte[] wrapData)
        {
            byte[] destinationArray = new byte[wrapData.Length - 1];
            Array.Copy(wrapData, 1, destinationArray, 0, destinationArray.Length);
            return destinationArray;
        }

    }
}
