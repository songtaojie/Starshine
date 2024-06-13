using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Starshine.Common
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 读取文件的字节数组
        /// </summary>
        /// <param name="fileFullPath">文件的全路径</param>
        /// <returns></returns>
        public static byte[] GetFile(string fileFullPath)
        {
            if (!File.Exists(fileFullPath))
                throw new DirectoryNotFoundException("找不到文件!");
            using (FileStream fileStream = new FileStream(fileFullPath, FileMode.Open))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);
                return buffer;
            }
        }
        /// <summary>
        /// 读取文件的文本
        /// </summary>
        /// <param name="fileFullPath">文件路径</param>
        /// <returns></returns>
        public static string GetString(string fileFullPath)
        {
            return GetString(fileFullPath, Encoding.UTF8);
        }
        /// <summary>
        /// 使用指定的编码读取文件的文本
        /// </summary>
        /// <param name="fileFullPath">文件路径</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string GetString(string fileFullPath, Encoding encoding)
        {
            if (!File.Exists(fileFullPath))
                throw new DirectoryNotFoundException("找不到文件!");
            return File.ReadAllText(fileFullPath, encoding);
        }
        //文件信息
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            return file;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fullFilePath">文件全路径</param>
        public static void RemoveFile(string fullFilePath)
        {
            try
            {
                File.Delete(fullFilePath);
            }
            catch (DirectoryNotFoundException)
            {
                return;
            }
            catch (IOException ex)
            {
                throw new IOException("文件正在使用中,无法删除文件!消息:" + ex.Message);
            }
        }
        /// <summary>
        /// 删除文件，不会报错
        /// </summary>
        /// <param name="path"></param>
        public static void TryDelete(string path)
        {
            try
            {
                if (ExistFile(path))
                {
                    File.Delete(path);
                }
            }
            catch { }

        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void TryCreateDirectory(string path)
        {
            if (ExistDirectory(path))
                return;
            Directory.CreateDirectory(path);
        }
        /// <summary>
        /// 判断是否存在文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ExistDirectory(string path)
        {
            return Directory.Exists(path);
        }
        /// <summary>
        /// 判断是否存在文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ExistFile(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 创建一个新文件，在其中写入指定的字节数组，然后关闭该文件。 如果目标文件已存在，则覆盖该文件
        /// </summary>
        /// <param name="path">要写入的文件。</param>
        /// <param name="bytes">要写入的字节数组</param>
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="sourceFileName">源文件全路径</param>
        /// <param name="destFileName">目标文件全路径</param>
        public static void TryCopy(string sourceFileName, string destFileName)
        {
            if (File.Exists(destFileName) || !File.Exists(sourceFileName))
                return;
            File.Copy(sourceFileName, destFileName);
        }

        #region 文件
        /// <summary>
        /// 根据文件扩展名称判断是否是图片文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsImageFile(string name)
        {
            Match match = Regex.Match(name, @"\.[^\.]+$");
            if (match.Success)
            {
                List<string> typeList = new List<string>(new string[] { ".bmp", ".png", ".gif", ".jpg", ".jpeg" });
                return typeList.Contains(match.Value.ToLower());
            }
            return false;
        }
        ///// <summary>
        ///// 根据文件名返回图片的文件格式
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static System.Drawing.Imaging.ImageFormat ImageFormat(string name)
        //{
        //    Match match = Regex.Match(name, @"\.[^\.]+$");
        //    if (match.Success)
        //    {
        //        string ma = match.Value.ToLower();
        //        List<string> typeList = new List<string>(new string[] { ".bmp", ".png", ".gif", ".jpg", ".jpeg" });
        //        if (ma == typeList[0]) return System.Drawing.Imaging.ImageFormat.Bmp;
        //        if (ma == typeList[1]) return System.Drawing.Imaging.ImageFormat.Png;
        //        if (ma == typeList[2]) return System.Drawing.Imaging.ImageFormat.Gif;
        //        if (ma == typeList[3] || ma == typeList[4]) return System.Drawing.Imaging.ImageFormat.Jpeg;
        //    }
        //    return System.Drawing.Imaging.ImageFormat.Bmp;
        //}
        /// <summary>
        /// 根据文件大小返回文件描述的文本
        /// 如1024返回1KB
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public static string FileSizeToShow(long fileSize)
        {
            if (fileSize >= 1024 * 1024 * 1024) return string.Format("{0:#,##0.##} GB", 1.0m * fileSize / (1024 * 1024 * 1024));
            if (fileSize >= 1024 * 1024) return string.Format("{0:0.##} MB", 1.0m * fileSize / (1024 * 1024));
            return string.Format("{0:0.###} KB", 1.0m * fileSize / 1024);
        }

        /// <summary>
        /// 根据字节大小获取文件的大小描述
        /// </summary>
        /// <param name="Size"></param>
        /// <returns></returns>
        public static string GetFileSizeDes(long Size)
        {
            string strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return strSize;
        }
        #endregion
    }
}
