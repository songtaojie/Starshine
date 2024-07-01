using System.Data;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 工作单元配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class UnitOfWorkAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnitOfWorkAttribute() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// <para>支持传入事务隔离级别 <see cref="IsolationLevel"/> 参数值</para>
        /// </remarks>
        /// <param name="isolationLevel">事务隔离级别</param>
        public UnitOfWorkAttribute(IsolationLevel isolationLevel)
        {
            IsolationLevel = isolationLevel;
        }
        /// <summary>
        /// 这是UOW事务性的吗?
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// 如果此UOW是事务性的，则此选项指示事务的隔离级别。 如果未提供，则使用默认值。
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// UOW超时时间(单位:毫秒)
        /// 如果未提供，则使用默认值。
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order => 9999;

        /// <summary>
        /// 用于防止启动该方法的工作单元。如果已经有启动的工作单元，则忽略此属性。
        /// 默认值:false。
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 设置option
        /// </summary>
        /// <param name="options"></param>
        public void SetOptions(UnitOfWorkOptions options)
        {
            if (Timeout.HasValue)
            {
                options.Timeout = Timeout;
            }

            if (IsolationLevel.HasValue)
            {
                options.IsolationLevel = IsolationLevel;
            }
        }
    }
}