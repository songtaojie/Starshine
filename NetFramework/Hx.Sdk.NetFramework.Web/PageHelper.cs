using System;
using System.Text;

namespace Hx.Sdk.NetFramework.Web
{
    /// <summary>
    /// Page页面的一些帮助类
    /// </summary>
    public static class PageHelper
    {
        /// <summary>
        /// 验证输入的数据转换特殊字符，并判断是否查过了最大限制
        /// </summary>
        /// <param name="inputText">输入的数据</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>
        public static string ValidateInputText(string inputText, int maxLength)
        {
            StringBuilder sb = new StringBuilder();
            inputText = inputText.Trim();
            if (maxLength > 0 && inputText.Length > maxLength)
                throw new Exception(string.Format("超过了该输入域的最大长度[{0}]", maxLength));
            //判断是否为空
            if (!string.IsNullOrEmpty(inputText))
            {
                //替换危险字符
                for (int i = 0, len = inputText.Length; i < len; i++)
                {
                    switch (inputText[i])
                    {
                        case '"':
                            sb.Append("&quot;");
                            break;
                        case '<':
                            sb.Append("&lt;");
                            break;
                        case '>':
                            sb.Append("&gt;");
                            break;
                        default:
                            sb.Append(inputText[i]);
                            break;
                    }
                }
                sb.Replace("'", " ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 验证字符串，转换危险字符
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public static string ValidateInputText(string inputText)
        {
            return ValidateInputText(inputText, -1);
        }
    }
}
