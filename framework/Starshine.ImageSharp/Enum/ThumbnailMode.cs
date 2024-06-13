namespace Starshine.ImageSharp
{
    /// <summary>
    /// 缩略图的模式
    /// </summary>
    public enum ThumbnailMode
    {
        /// <summary>
        /// 指定高宽缩放,拉伸（会变形） 
        /// </summary>
        Stretch,
        /// <summary>
        ///调整图像大小，直到最短的一面达到设定的给定尺寸。 
        ///在此模式下禁用升频功能，如果尝试则将返回原始图像。
        /// </summary>
        Min,
        /// <summary>
        ///约束调整大小的图像以适合其容器的边界，并保持原始宽高比。
        /// </summary>
        Max,
        /// <summary>
        ///填充图像以适合容器的边界，而无需调整原始源的大小。
        /// </summary>
        BoxPad
    }
}
