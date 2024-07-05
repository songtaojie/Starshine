// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Starshine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 带更新时间的配置
/// </summary>
/// <typeparam name="T">实体类型</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class AuditedEntityBaseTypeConfiguration<T, TKey> : CreationEntityBaseTypeConfiguration<T, TKey>
     where T : AuditedEntityBase<TKey>
{
    /// <summary>
    /// 应用配置
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.UpdateTime).HasComment("最后修改时间");
        builder.Property(x => x.UpdaterId).HasComment("最后修改人id");
    }
}

/// <summary>
/// 带更新时间的配置
/// </summary>
/// <typeparam name="T">实体类型</typeparam>
public abstract class AuditedEntityBaseTypeConfiguration<T> : AuditedEntityBaseTypeConfiguration<T,long>
     where T : AuditedEntityBase
{
    /// <summary>
    /// 应用配置
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
    }
}