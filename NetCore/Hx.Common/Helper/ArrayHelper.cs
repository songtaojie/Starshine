using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hx.Common
{
    /// <summary>
    /// 数组帮助类
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// 把流转换为字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>字节数组</returns>
        public static byte[] ReadAllBytesFromStream(Stream stream)
        {
            MemoryStream memory = new MemoryStream();
            byte[] buffer = new byte[100];
            int count = stream.Read(buffer, 0, 100);
            while (count != 0)
            {
                //if (count == 0)
                //{
                //    return memory.ToArray();
                //}
                memory.Write(buffer, 0, count);
                count = stream.Read(buffer, 0, 100);
            }
            byte[] result = memory.ToArray();
            memory.Dispose();
            return result;
        }
        /// <summary>
        /// 移除数组中的某一项，并返回剩余项的数组
        /// </summary>
        /// <typeparam name="T">泛型，数组类型</typeparam>
        /// <param name="array">原数组</param>
        /// <param name="item">要移除的项</param>
        /// <returns>移除指定项后的数组</returns>
        public static T[] RemoveArrayItem<T>(T[] array, T item)
        {
            if (item != null)
            {
                if (array == null)
                {
                    return null;
                }
                List<T> list = new List<T>(array);
                list.Remove(item);
                return list.ToArray();
            }
            return array;
        }
        /// <summary>
        /// 把两个数组进行合并
        /// </summary>
        /// <typeparam name="T">泛型，数组的类型</typeparam>
        /// <param name="array1">第一个数组</param>
        /// <param name="array2">第二个数组</param>
        /// <returns>合并后的数组</returns>
        public static T[] ArrayMerge<T>(T[] array1, T[] array2)
        {
            T[] array = new T[array1.Length + array2.Length];
            array1.CopyTo(array, 0);
            array2.CopyTo(array, array1.Length);
            return array;
        }
        /// <summary>
        /// 向数组中添加项
        /// </summary>
        /// <typeparam name="T">泛型，数组的类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="item">要添加的项</param>
        /// <returns>添加指定项后的数组</returns>
        public static T[] AddArrayItem<T>(T[] array, T item)
        {
            List<T> list = null;
            if (item != null)
            {
                if (array != null)
                {
                    list = new List<T>(array);
                }
                else
                {
                    list = new List<T>();
                }
                list.Add(item);
                array = list.ToArray();
            }
            return array;
        }
        /// <summary>
        /// 向数组中添加多个项
        /// </summary>
        /// <typeparam name="T">泛型，数组的类型</typeparam>
        /// <param name="array1">原数组</param>
        /// <param name="array2">添加的数组的项</param>
        /// <returns>添加后的数组</returns>
        public static T[] AddArrayItem<T>(T[] array1, params T[] array2)
        {
            List<T> list = new List<T>();
            if (array1 != null && array1.Length != 0)
            {
                for (int index = 0; index < array1.Length; index++)
                {
                    list.Add(array1[index]);
                }
            }
            if (array2 != null && array2.Length != 0)
            {
                for (int index = 0; index < array2.Length; index++)
                {
                    list.Add(array2[index]);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 把数组使用指定的分隔符进行分割
        /// </summary>
        /// <typeparam name="T">泛型，数组类型</typeparam>
        /// <param name="arrayList">数组</param>
        /// <param name="separator">分隔符</param>
        /// <returns>分割后的字符串</returns>
        public static string JoinString<T>(T[] arrayList, string separator)
        {
            if (arrayList != null && arrayList.Length != 0)
            {
                string[] tempList = new string[arrayList.Length];
                for (int index = 0; index < arrayList.Length; index++)
                {
                    tempList[index] = string.Format("{0}", arrayList[index]);
                }
                return string.Join(separator, tempList);
            }
            return null;
        }
    }
}
