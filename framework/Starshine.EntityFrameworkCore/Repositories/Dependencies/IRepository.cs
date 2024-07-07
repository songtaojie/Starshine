// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

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