// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 
/// </summary>
public static class StarshineModelBuilderExtensions
{
    private const string ModelDatabaseProviderAnnotationKey = "_Starshine_DatabaseProvider";

    /// <summary>
    /// 设置databaseProvider
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="databaseProvider"></param>
    public static void SetDatabaseProvider(this ModelBuilder modelBuilder,EFCoreDatabaseProvider databaseProvider)
    {
        modelBuilder.Model.SetAnnotation(ModelDatabaseProviderAnnotationKey, databaseProvider);
    }
    /// <summary>
    /// 清除databaseProvider
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void ClearDatabaseProvider(this ModelBuilder modelBuilder)
    {
        modelBuilder.Model.RemoveAnnotation(ModelDatabaseProviderAnnotationKey);
    }

    /// <summary>
    /// 获取databaseProvider
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
    public static EFCoreDatabaseProvider? GetDatabaseProvider(this ModelBuilder modelBuilder)
    {
        return modelBuilder.Model[ModelDatabaseProviderAnnotationKey] as EFCoreDatabaseProvider?;
    }
}
