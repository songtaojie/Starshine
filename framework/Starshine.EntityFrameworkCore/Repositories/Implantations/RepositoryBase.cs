using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Starshine.Common;
using System;
using System.Collections.Generic;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    ///仓储基础
    /// </summary>
    public abstract class RepositoryBase: IRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scoped">服务提供器</param>
        public RepositoryBase(IServiceProvider scoped)
        {
            ServiceProvider = scoped;
        }

        /// <summary>
        /// 服务提供器
        /// </summary>
        public virtual IServiceProvider ServiceProvider{ get; }

        /// <summary>
        /// 是否跟踪
        /// </summary>
        public virtual bool? IsChangeTrackingEnabled { get; protected set; }
    }
}