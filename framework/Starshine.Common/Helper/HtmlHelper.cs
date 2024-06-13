using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Starshine.Common
{
    /// <summary>
    /// Html帮助类
    /// </summary>
    public static class HtmlHelper
    {

        #region Html
        /// <summary>
        ///从html文本中获取图片链接
        /// </summary>
        /// <param name="sHtmlText"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetHtmlImageUrlList(string sHtmlText)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(sHtmlText)) return result;
            // 定义正则表达式用来匹配 img 标签   
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(sHtmlText);

            // 取得匹配项列表   
            foreach (Match match in matches)
            {
                result.Add(match.Groups["imgUrl"].Value);
            }
            return result;
        }

        /// <summary>
        /// 过滤html中的p标签
        /// </summary>
        /// <param name="html">html字符串</param>
        /// <param name="maxSize">返回的字符串最大长度为多少</param>
        /// <param name="onlyText">是否只返回纯文本，还是返回带有标签的</param>
        /// <returns></returns>
        public static string FilterHtml(string html, int maxSize, bool onlyText = true)
        {
            if (string.IsNullOrEmpty(html)) return "";

            //删除脚本
            html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            html.Replace("<", "");
            html.Replace(">", "");
            html.Replace("\r\n", "");

            //Regex rReg = new Regex(@"<P>[\s\S]*?</P>", RegexOptions.IgnoreCase);
            //var matchs = Regex.Matches(html, @"<P>[\s\S]*?</P>", RegexOptions.IgnoreCase);
            //StringBuilder sb = new StringBuilder();
            //foreach (Match match in matchs)
            //{
            //    string pContent = match.Value;
            //    if (onlyText)
            //    {
            //        pContent = Regex.Replace(pContent, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //        pContent = Regex.Replace(pContent, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            //    }
            //    sb.Append(pContent);
            //}
            //string result = sb.ToString();
            if (html.Length < maxSize) return html;
            return html.Substring(0, maxSize);
        }
        #endregion
    }
}
