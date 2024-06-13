using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Starshine.Common.Serialization
{
    /// <summary>
    /// 序列化类
    /// </summary>
    public class Serializer
    {
        private BinaryFormatter bf;
        private Compressor compressor;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Serializer()
        {
            this.bf = new BinaryFormatter();
            this.compressor = new Compressor();
        }
        /// <summary>
        /// 初始化序列化对象，设置序列化时字节数组的最大长度，如果超出该长度
        /// 则对字节数组进行压缩，默认overSIze=0x3e8
        /// </summary>
        /// <param name="overSize"></param>
        public Serializer(int overSize)
        {
            this.bf = new BinaryFormatter();
            this.compressor = new Compressor(overSize);
        }
        /// <summary>
        /// 把字节数组进行反序列化为指定的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public T BinaryDeserialize<T>(byte[] data)
        {
            if (data == null)
            {
                return default(T);
            }
            MemoryStream serializationStream = new MemoryStream(this.compressor.Decompress(data));
            return (T)this.bf.Deserialize(serializationStream);
        }
        /// <summary>
        /// 把对象序列化为字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte[] BinarySerialize<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            MemoryStream serializationStream = new MemoryStream();
            this.bf.Serialize(serializationStream, obj);
            return this.compressor.Compress(serializationStream.ToArray());
        }

    }
}
