using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Common
{
    /// <summary>
    /// 控制台帮助类
    /// </summary>
    public static class ConsoleHelper
    {
        static void WriteColorLine(string str, ConsoleColor color,bool newLine = false)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = currentForeColor;
            if (newLine) Console.WriteLine();
        }

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否在后面添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteErrorLine(this string str, bool newLine = false, ConsoleColor color = ConsoleColor.Red)
        {
            WriteColorLine(str, color, newLine);
        }

        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否在后面添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteWarningLine(this string str, bool newLine = false, ConsoleColor color = ConsoleColor.Yellow)
        {
            WriteColorLine(str, color, newLine);
        }
        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否在后面添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteInfoLine(this string str, bool newLine = false, ConsoleColor color = ConsoleColor.White)
        {
            WriteColorLine(str, color, newLine);
        }
        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否在后面添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteSuccessLine(this string str, bool newLine = false,ConsoleColor color = ConsoleColor.Green)
        {
            WriteColorLine(str, color, newLine);
        }

    }
}
