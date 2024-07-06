// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;

/// <summary>
/// 假删除/软删除
/// </summary>
[AttributeUsage(AttributeTargets.Class,AllowMultiple =false)]
public class EntityContextMarkerAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContextType"></param>
    public EntityContextMarkerAttribute(Type dbContextType)
    {
        if (!typeof(DbContext).IsAssignableFrom(dbContextType))
        {
            throw new ArgumentException($"Type {dbContextType.FullName} must implement {typeof(DbContext).FullName}.");
        }
        DbContextType = dbContextType;
    }

    /// <summary>
    /// 假删除/软删除状态
    /// </summary>
    public Type DbContextType { get; set; }
}