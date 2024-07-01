using Starshine.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Starshine.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 非泛型仓储
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// 服务提供器
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 是否启用追踪
        /// </summary>
        bool? IsChangeTrackingEnabled { get; }
    }
}