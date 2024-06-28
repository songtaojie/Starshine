namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// 配置表名称前缀
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TablePreSuffixAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prefix"></param>
        public TablePreSuffixAttribute(string prefix)
        {
            Prefix = prefix;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="suffix"></param>
        public TablePreSuffixAttribute(string prefix, string suffix)
            : this(prefix)
        {
            Suffix = suffix;
        }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 表统一后缀
        /// </summary>
        public string? Suffix { get; set; }
    }
}