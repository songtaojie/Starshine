using SixLabors.Fonts;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.ImageSharp
{
    /// <summary>
    /// 水印字体的配置
    /// </summary>
    public sealed class FontOptions
    {
        /// <summary>
        /// family name，默认是Arial
        /// </summary>
        public string FontFamily { get; set; } = "Arial";

        /// <summary>
        /// 字体大小,默认是10
        /// </summary>
        public int FontSize { get; set; } = 10;

        /// <summary>
        /// 字体颜色，默认是黑色
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// 水印字体的位置，默认在右下角
        /// </summary>
        public WaterLocation WaterLocation { get; set; } = WaterLocation.RightBottom;

        /// <summary>
        /// 自动换行
        /// </summary>
        public bool Wordwrap { get; set; } = false;

        /// <summary>
        /// Padding,默认为5
        /// </summary>
        public float Padding { get; set; } = 5F;
        /// <summary>
        /// 水印文字
        /// </summary>
        public string Letter { get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="letter">水印文字</param>
        public FontOptions(string letter)
        {
            this.Letter = letter;
        }
        /// <summary>
        /// 字体
        /// </summary>
        internal Font Font
        {
            get
            {
                //对于生产应用程序，我们建议您创建一个FontCollection单例并自己手动安装ttf字体，
                // 因为使用SystemFonts可能会很昂贵，并且您可能会因部署而异，因此存在存在字体或不存在字体的风险。
                //var fonts = new FontCollection();
                //var fontFamily = fonts.Install("./{FontFamily}.ttf");
                bool isExist = SystemFonts.TryGet(FontFamily, out FontFamily fontFamily);
                if (!isExist) throw new Exception($"该字体【{FontFamily}】未安装在字体集中。");
                if (FontSize <= 0) throw new Exception("字体大小不能小于0");
                //用于缩放水印大小的方法在很大程度上被忽略了。
                return SystemFonts.CreateFont(FontFamily, FontSize);
            }
        }
    }
}
