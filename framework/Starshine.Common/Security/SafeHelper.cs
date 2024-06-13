using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hx.Common.Security
{
    /// <summary>
    /// 一些加密解密的操作类
    /// </summary>
    public static class SafeHelper
    {
        #region Des加解密
        private const string EncryptKey = "60WE4U(7";
        /// <summary>
        /// 进行DES加密
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <param name="sKey">密钥，必须为8位</param>
        /// <returns>以Base64格式返回的加密字符串</returns>
        public static string DESEncrypt(string pToEncrypt, string sKey = EncryptKey)
        {
            sKey = GetKey(sKey, 8);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        /// <summary>
        /// 进行DES解密
        /// </summary>
        /// <param name="pToDecrypt">要解密的字符串</param>
        /// <param name="sKey">密钥，必须为8位</param>
        /// <returns>返回解密后的字符串</returns>
        public static string DESDecrypt(string pToDecrypt, string sKey = EncryptKey)
        {
            sKey = GetKey(sKey, 8);
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        private static string GetKey(string sKey, int length)
        {
            if (sKey.Length > length)
                sKey = sKey.Substring(0, length);
            else if (sKey.Length < length)
                sKey = sKey.PadRight(length, ' ');
            return sKey;
        }
        #endregion


        #region MD5加密
        /// <summary>
        /// 32位的MD5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Md5Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            using (var md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                var result = new StringBuilder();
                foreach (byte t in bytes)
                {
                    result.Append(t.ToString("X2"));
                }
                return result.ToString();
            }
        }
        #endregion
    }
}
