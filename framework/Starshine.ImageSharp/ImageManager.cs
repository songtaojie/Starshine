using SixLabors.ImageSharp.Drawing;
using System;
using System.IO;
using System.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using Hx.ImageSharp.Fonts;
using System.Collections.Concurrent;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace Hx.ImageSharp
{
    /// <summary>
    /// image帮助类
    /// </summary>
    public static class ImageManager
    {
        private static readonly string[] _imageExtList = new string[] { ".gif", ".jpg", ".jpeg", ".png", ".bmp", ".icon" };
        private static readonly ConcurrentDictionary<string, Font> _fontDic = new ConcurrentDictionary<string, Font>();
        /// <summary>
        /// 根据文件流判断是否是图片
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static bool IsImage(Stream stream)
        {
            bool result = true;
            try
            {
               Image.Load(stream);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 根据扩展名判断是否是图片
        /// </summary>
        /// <param name="ext">文件扩展</param>
        /// <param name="hasPoint">是否包含点</param>
        /// <returns></returns>
        public static bool IsImage(string ext, bool hasPoint = true)
        {
            ext = string.IsNullOrEmpty(ext) ? ext : ext.ToLower();
            return hasPoint ? _imageExtList.Contains(ext) : _imageExtList.Contains("." + ext);
        }

        /// <summary>
        /// 获取图片大大小
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        public static void GetImageSize(string imagePath, out int width, out int height)
        {
            using (Image<Rgba32> originalImage = Image.Load<Rgba32>(imagePath))
            {
                width = originalImage.Width;
                height = originalImage.Height;
            }
        }

        /// <summary>
        /// 获取图片大大小
        /// </summary>
        /// <param name="stream">图片的流数据</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        public static void GetImageSize(Stream stream, out int width, out int height)
        {
            using (Image<Rgba32> originalImage = Image.Load<Rgba32>(stream))
            {
                width = originalImage.Width;
                height = originalImage.Height;
            }
        }
        #region 缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ThumbnailMode mode)
        {
            ResizeOptions options = new ResizeOptions();

            using (Image<Rgba32> originalImage = Image.Load<Rgba32>(originalImagePath))
            {
                int towidth = width;
                int toheight = height;
                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;
                switch (mode)
                {
                    case ThumbnailMode.Stretch:  //指定高宽缩放（可能变形） 
                        options.Mode = ResizeMode.Stretch;
                        break;
                    case ThumbnailMode.Max:   //指定宽，高按比例   
                        options.Mode = ResizeMode.Max;
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case ThumbnailMode.Min:   //指定高，宽按比例
                        options.Mode = ResizeMode.Min;
                        options.Position = AnchorPositionMode.BottomLeft;
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case ThumbnailMode.BoxPad: //指定高宽裁减（不变形） 
                        options.Mode = ResizeMode.BoxPad;
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        options.Mode = ResizeMode.Max;
                        break;
                }
                options.Size = new Size(width, height);
                originalImage.Mutate(x => x.Resize(options));
                originalImage.Save(thumbnailPath);
            }
        }
        #endregion

        #region 文字水印
        /// <summary>
        /// 文字水印处理方法
        /// </summary>
        /// <param name="path">图片路径（绝对路径）</param>
        /// <param name="fontOptions">字体的配置</param>
        public static string MarkLetterWater(string path, Fonts.FontOptions fontOptions)
        {
            #region
            string fileExt = System.IO.Path.GetExtension(path);
            if (_imageExtList.Contains(fileExt))
            {
                DateTime time = DateTime.Now;
                string filename = time.ToString("yyyyMMddHHmmssffff");
                string newpath = $"{System.IO.Path.GetDirectoryName(path)}{filename}{_imageExtList}";
                //原始的img对象完全没有改变。
                using Image<Rgba32> image = Image.Load<Rgba32>(path);
                if (fontOptions.Wordwrap)
                {
                    using var img2 = image.Clone(ctx => ctx.ApplyScalingWaterMarkWordWrap(fontOptions));
                    img2.Save(newpath);
                }
                else
                {
                    using var img2 = image.Clone(ctx => ctx.ApplyScalingWaterMarkSimple(fontOptions));
                    img2.Save(newpath);
                }
                return newpath;
            }
            return path;
            #endregion
        }

        /// <summary>
        /// 文字水印位置的方法
        /// </summary>
        /// <param name="fontOptions">字体配置</param>
        /// <param name="imgSize">图片的大小</param>
        /// <param name="width">宽(当水印类型为文字时,传过来的就是字体的大小)</param>
        /// <param name="height">高(当水印类型为文字时,传过来的就是字符的长度)</param>
        /// <returns>返回的是水印的位置</returns>
        private static (TextOptions, SixLabors.ImageSharp.PointF) GetLocation(FontOptions fontOptions, SixLabors.ImageSharp.Size imgSize, float width, float height)
        {
             TextOptions textOptions = new TextOptions(fontOptions.Font);
            float x = 10;
            float y = 10;
            SixLabors.ImageSharp.PointF pointF = new SixLabors.ImageSharp.PointF(x, y);
            switch (fontOptions.WaterLocation)
            {
                case WaterLocation.LeftTop:
                    textOptions.HorizontalAlignment = HorizontalAlignment.Left;
                    textOptions.VerticalAlignment = VerticalAlignment.Bottom;
                    if (fontOptions.Wordwrap)
                    {
                        pointF.X = fontOptions.Padding;
                    }
                    break;
                case WaterLocation.Top:
                    textOptions.HorizontalAlignment = HorizontalAlignment.Center;
                    textOptions.VerticalAlignment = VerticalAlignment.Bottom;
                    if (fontOptions.Wordwrap)
                    {
                        x = imgSize.Width / 2 - width / 2 + fontOptions.Padding;
                    }
                    else
                    {
                        x = imgSize.Width / 2 - width / 2 + fontOptions.Padding;
                    }
                    pointF.X = x;
                    break;
                case WaterLocation.RightTop:
                    textOptions.HorizontalAlignment = HorizontalAlignment.Right;
                    textOptions.VerticalAlignment = VerticalAlignment.Bottom;
                    if (fontOptions.Wordwrap)
                    {
                        x = imgSize.Width - width + fontOptions.Padding;
                    }
                    else
                    {
                        x = imgSize.Width - width;
                    }
                    pointF.X = x;
                    break;
                case WaterLocation.LeftCenter:
                    textOptions.HorizontalAlignment = HorizontalAlignment.Left;
                    textOptions.VerticalAlignment = VerticalAlignment.Center;
                    if (fontOptions.Wordwrap)
                    {
                        x = fontOptions.Padding;
                        pointF.X = x;
                    }
                    y = imgSize.Height / 2;
                    pointF.Y = y;
                    break;
                case WaterLocation.Center:
                    textOptions.HorizontalAlignment = HorizontalAlignment.Center;
                    textOptions.VerticalAlignment = VerticalAlignment.Center;
                    x = imgSize.Width / 2 - width / 2;
                    y = imgSize.Height / 2;
                    if (fontOptions.Wordwrap) x += fontOptions.Padding;
                    pointF.X = x;
                    pointF.Y = y;
                    break;
                case WaterLocation.RightCenter:
                    textOptions.HorizontalAlignment = HorizontalAlignment.Right;
                    textOptions.VerticalAlignment = VerticalAlignment.Center;
                    x = imgSize.Width - width;
                    y = imgSize.Height / 2;
                    if (fontOptions.Wordwrap) x += fontOptions.Padding;
                    pointF.X = x;
                    pointF.Y = y;
                    break;
                case WaterLocation.LeftBottom:
                    textOptions.HorizontalAlignment = HorizontalAlignment.Left;
                    textOptions.VerticalAlignment = VerticalAlignment.Top;
                    y = imgSize.Height - height - 5;
                    if (fontOptions.Wordwrap) pointF.X = fontOptions.Padding;
                    pointF.Y = y;
                    break;
                case WaterLocation.Bottom:
                    textOptions.HorizontalAlignment = HorizontalAlignment.Center;
                    textOptions.VerticalAlignment = VerticalAlignment.Top;
                    x = imgSize.Width / 2 - width / 2;
                    y = imgSize.Height - height - 5;
                    if (fontOptions.Wordwrap)
                    {
                        x += fontOptions.Padding;
                        y += fontOptions.Padding;
                    }
                    pointF.X = x;
                    pointF.Y = y;
                    break;
                case WaterLocation.RightBottom:
                    textOptions.HorizontalAlignment = HorizontalAlignment.Right;
                    textOptions.VerticalAlignment = VerticalAlignment.Top;
                    x = imgSize.Width - width;
                    y = imgSize.Height - height - 5;
                    if (fontOptions.Wordwrap)
                    {
                        x += fontOptions.Padding;
                        y += fontOptions.Padding;
                    }
                    pointF.X = x;
                    pointF.Y = y;
                    break;
            }
            return (textOptions, pointF);
        }

        /// <summary>
        /// 获取字体
        /// </summary>
        /// <param name="fontOptions"></param>
        /// <returns></returns>
        private static Font GetFont(FontOptions fontOptions)
        {
            Font font;
            var fontFamily = fontOptions.FontFamily.ToLower();
            if (_fontDic.ContainsKey(fontFamily))
            {
                font = _fontDic[fontFamily];
            }
            else
            {
                font = fontOptions.Font;
                _fontDic[fontFamily] = font;
            }
            return font;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processingContext"></param>
        /// <param name="fontOptions">字体配置</param>
        /// <returns></returns>
        private static IImageProcessingContext ApplyScalingWaterMarkSimple(this IImageProcessingContext processingContext,
            FontOptions fontOptions)
        {
            Size imgSize = processingContext.GetCurrentSize();

            //float targetWidth = imgSize.Width - (padding * 2);
            //float targetHeight = imgSize.Height - (padding * 2);
           
            var font = GetFont(fontOptions);
            // 测量文字大小
            FontRectangle size = TextMeasurer.Measure(fontOptions.Letter, new TextOptions(font));
            //找出我们需要缩放文本以填充空间的大小（上下）
            float scalingFactor = Math.Min(imgSize.Width / size.Width, imgSize.Height / size.Height);
            //创建一个新字体
            Font scaledFont = new Font(font, scalingFactor * font.Size);
            var testOptions = GetLocation(fontOptions, imgSize, size.Width, size.Height);
            return processingContext.DrawText(new DrawingOptions(),fontOptions.Letter, scaledFont, fontOptions.Color, testOptions.Item2);
        }

        private static IImageProcessingContext ApplyScalingWaterMarkWordWrap(this IImageProcessingContext processingContext,
            FontOptions fontOptions)
        {
            var padding = fontOptions.Padding;
            SixLabors.ImageSharp.Size imgSize = processingContext.GetCurrentSize();
            float targetWidth = imgSize.Width - (padding * 2);
            float targetHeight = imgSize.Height - (padding * 2);

            float targetMinHeight = imgSize.Height - (padding * 3); // 必须位于目标高度的边距宽度内

            //现在我们正在一次处理2维，不能只是缩放，因为它将导致文本重排，我们需要尝试多次

            var scaledFont = GetFont(fontOptions);
            FontRectangle s = new FontRectangle(0, 0, float.MaxValue, float.MaxValue);

            float scaleFactor = scaledFont.Size / 2; // 每当我们改变方向时，我们将这个大小减半
            int trapCount = (int)scaledFont.Size * 2;
            if (trapCount < 10)
            {
                trapCount = 10;
            }

            bool isTooSmall = false;

            while ((s.Height > targetHeight || s.Height < targetMinHeight) && trapCount > 0)
            {
                if (s.Height > targetHeight)
                {
                    if (isTooSmall)
                    {
                        scaleFactor /= 2;
                    }

                    scaledFont = new Font(scaledFont, scaledFont.Size - scaleFactor);
                    isTooSmall = false;
                }

                if (s.Height < targetMinHeight)
                {
                    if (!isTooSmall)
                    {
                        scaleFactor /= 2;
                    }
                    scaledFont = new Font(scaledFont, scaledFont.Size + scaleFactor);
                    isTooSmall = true;
                }
                trapCount--;
                s = TextMeasurer.Measure(fontOptions.Letter, new TextOptions(scaledFont)
                {
                    TabWidth = targetWidth
                    //WrappingWidth = targetWidth
                });
            }
            var textOptions = GetLocation(fontOptions, imgSize, s.Width, s.Height);
            var textGraphicOptions = textOptions.Item1;
            //textGraphicOptions.TextOptions.WrapTextWidth = targetWidth;
            var center = textOptions.Item2;
            //center.X = padding;
            //var center = new PointF(padding, imgSize.Height / 2);
            //var textGraphicOptions = new TextGraphicsOptions()
            //{
            //    TextOptions = {
            //        HorizontalAlignment = HorizontalAlignment.Left,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        WrapTextWidth = targetWidth
            //    }
            //};
            return processingContext.DrawText(new DrawingOptions(), fontOptions.Letter, scaledFont, fontOptions.Color, center);
        }

        #endregion
    }

}
