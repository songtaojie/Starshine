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
/// 带删除信息的配置
/// </summary>
/// <typeparam name="T">实体类型</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class FullAuditedEntityBaseTypeConfiguration<T, TKey> : AuditedEntityBaseTypeConfiguration<T, TKey>
     where T : FullAuditedEntityBase<TKey>
{
    /// <summary>
    /// 应用配置
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.IsDeleted).HasComment("是否删除");
        builder.Property(x => x.DeleterId).HasComment("删除人id");
        builder.Property(x => x.DeleteTime).HasComment("删除时间");
    }
}

/// <summary>
/// 带删除信息的配置
/// </summary>
/// <typeparam name="T">实体类型</typeparam>
public abstract class FullAuditedEntityBaseTypeConfiguration<T> : AuditedEntityBaseTypeConfiguration<T>
     where T : FullAuditedEntityBase
{
    /// <summary>
    /// 应用配置
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.IsDeleted).HasComment("是否删除");
        builder.Property(x => x.DeleterId).HasComment("删除人id");
        builder.Property(x => x.DeleteTime).HasComment("删除时间");
    }
}