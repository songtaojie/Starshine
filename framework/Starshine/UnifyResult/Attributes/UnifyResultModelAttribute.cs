using System;

namespace Starshine
{
    /// <summary>
    /// 规范化模型特性
    /// </summary>
    [SkipScan, AttributeUsage(AttributeTargets.Class)]
    public class UnifyResultModelAttribute : Attribute
    {
        /// <summary>
        /// 规范化模型
        /// </summary>
        /// <param name="modelType">模型的类型</param>
        public UnifyResultModelAttribute(Type modelType)
        {
            ModelType = modelType;
        }

        /// <summary>
        /// 模型类型
        /// </summary>
        public Type ModelType { get; set; }
    }
}