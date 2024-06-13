// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Sqlsugar;
public static class RepositoryExtension
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    internal static IEnumerable<DbConnectionConfig>? ConnectionConfigs { get; set; }
    /// <summary>
    /// 实体假删除 _rep.FakeDelete(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static int FakeDelete<T>(this ISugarRepository repository, T entity) where T : FullAuditedEntityBase, new()
    {
        return repository.Context.FakeDelete(entity);
    }

    /// <summary>
    /// 实体假删除 db.FakeDelete(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static int FakeDelete<T>(this ISqlSugarClient db, T entity) where T : FullAuditedEntityBase, new()
    {
        return db.Updateable(entity).AS()
            .ReSetValue(u =>
            {
                u.IsDeleted = true;
                u.DeleteTime = DateTime.Now;
                u.UpdateTime = DateTime.Now;
            })
            .IgnoreColumns(ignoreAllNullColumns: true)
            .EnableDiffLogEvent()
            .UpdateColumns(u => new { u.IsDeleted, u.DeleteTime, u.DeleterId, u.UpdateTime, u.UpdaterId })
            .ExecuteCommand();
    }

    /// <summary>
    /// 实体假删除异步 _rep.FakeDeleteAsync(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Task<int> FakeDeleteAsync<T>(this ISugarRepository repository, T entity) where T : FullAuditedEntityBase, new()
    {
        return repository.Context.FakeDeleteAsync(entity);
    }

    /// <summary>
    /// 实体假删除 db.FakeDelete(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Task<int> FakeDeleteAsync<T>(this ISqlSugarClient db, T entity) where T : FullAuditedEntityBase, new()
    {
        return db.Updateable(entity).AS().ReSetValue(u =>
            {
                u.IsDeleted = true;
                u.DeleteTime = DateTime.Now;
                u.UpdateTime = DateTime.Now;
            })
            .IgnoreColumns(ignoreAllNullColumns: true)
            .EnableDiffLogEvent()
            .UpdateColumns(u => new { u.IsDeleted, u.DeleteTime, u.DeleterId, u.UpdateTime, u.UpdaterId })
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 排序方式(默认降序)
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="pageInput"> </param>
    /// <param name="defualtSortField"> 默认排序字段 </param>
    /// <param name="descSort"> 是否降序 </param>
    /// <returns> </returns>
    public static ISugarQueryable<T> OrderBuilder<T>(this ISugarQueryable<T> queryable, BasePageParam pageInput, string defualtSortField = "Id", bool descSort = true)
    {
        var orderStr = "";
        // 约定默认每张表都有Id排序
        if (!string.IsNullOrWhiteSpace(defualtSortField))
        {
            orderStr = descSort ? defualtSortField + " Desc" : defualtSortField + " Asc";
        }

        // 排序是否可用-排序字段和排序顺序都为非空才启用排序
        if (!string.IsNullOrEmpty(pageInput.SortField)
            && ConnectionConfigs != null && ConnectionConfigs!.Any())
        {
            var config = ConnectionConfigs!.FirstOrDefault(u => u.ConfigId == queryable.Context.CurrentConnectionConfig.ConfigId);
            var field = config != null && config.EnableUnderLine ? UtilMethods.ToUnderLine(pageInput.SortField) : pageInput.SortField;
            orderStr = $"{field} {(pageInput.OrderType == OrderTypeEnum.DESC ? "Desc" : "Asc")}";
        }
        return queryable.OrderByIF(!string.IsNullOrWhiteSpace(orderStr), orderStr);
    }

    /// <summary>
    /// 更新实体并记录差异日志 _rep.UpdateWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <param name="ignoreAllNullColumns"></param>
    /// <returns></returns>
    public static int UpdateWithDiffLog<T>(this ISugarRepository repository, T entity, bool ignoreAllNullColumns = true) where T : EntityBase, new()
    {
        return repository.Context.UpdateWithDiffLog(entity, ignoreAllNullColumns);
    }

    /// <summary>
    /// 更新实体并记录差异日志 _rep.UpdateWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <param name="ignoreAllNullColumns"></param>
    /// <returns></returns>
    public static int UpdateWithDiffLog<T>(this ISqlSugarClient db, T entity, bool ignoreAllNullColumns = true) where T : EntityBase, new()
    {
        return db.Updateable(entity).AS()
            .IgnoreColumns(ignoreAllNullColumns: ignoreAllNullColumns)
            .EnableDiffLogEvent()
            .ExecuteCommand();
    }

    /// <summary>
    /// 更新实体并记录差异日志 _rep.UpdateWithDiffLogAsync(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <param name="ignoreAllNullColumns"></param>
    /// <returns></returns>
    public static Task<int> UpdateWithDiffLogAsync<T>(this ISugarRepository repository, T entity, bool ignoreAllNullColumns = true) where T : EntityBase, new()
    {
        return repository.Context.UpdateWithDiffLogAsync(entity, ignoreAllNullColumns);
    }

    /// <summary>
    /// 更新实体并记录差异日志 _rep.UpdateWithDiffLogAsync(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <param name="ignoreAllNullColumns"></param>
    /// <returns></returns>
    public static Task<int> UpdateWithDiffLogAsync<T>(this ISqlSugarClient db, T entity, bool ignoreAllNullColumns = true) where T : EntityBase, new()
    {
        return db.Updateable(entity)
            .IgnoreColumns(ignoreAllNullColumns: ignoreAllNullColumns)
            .EnableDiffLogEvent()
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 新增实体并记录差异日志 _rep.InsertWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static int InsertWithDiffLog<T>(this ISugarRepository repository, T entity) where T : EntityBase, new()
    {
        return repository.Context.InsertWithDiffLog(entity);
    }

    /// <summary>
    /// 新增实体并记录差异日志 _rep.InsertWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static int InsertWithDiffLog<T>(this ISqlSugarClient db, T entity) where T : EntityBase, new()
    {
        return db.Insertable(entity).AS().EnableDiffLogEvent().ExecuteCommand();
    }

    /// <summary>
    /// 新增实体并记录差异日志 _rep.InsertWithDiffLogAsync(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Task<int> InsertWithDiffLogAsync<T>(this ISugarRepository repository, T entity) where T : EntityBase, new()
    {
        return repository.Context.InsertWithDiffLogAsync(entity);
    }

    /// <summary>
    /// 新增实体并记录差异日志 _rep.InsertWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Task<int> InsertWithDiffLogAsync<T>(this ISqlSugarClient db, T entity) where T : EntityBase, new()
    {
        return db.Insertable(entity).AS().EnableDiffLogEvent().ExecuteCommandAsync();
    }

    /// <summary>
    /// 多库查询
    /// </summary>
    /// <param name="queryable"></param>
    /// <returns> </returns>
    public static ISugarQueryable<T> AS<T>(this ISugarQueryable<T> queryable)
    {
        var info = GetTableInfo<T>();
        return queryable.AS<T>($"{info.Item1}.{info.Item2}");
    }

    /// <summary>
    /// 多库查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="queryable"></param>
    /// <returns></returns>
    public static ISugarQueryable<T, T2> AS<T, T2>(this ISugarQueryable<T, T2> queryable)
    {
        var info = GetTableInfo<T2>();
        return queryable.AS<T2>($"{info.Item1}.{info.Item2}");
    }

    /// <summary>
    /// 多库更新
    /// </summary>
    /// <param name="updateable"></param>
    /// <returns> </returns>
    public static IUpdateable<T> AS<T>(this IUpdateable<T> updateable) where T : EntityBase, new()
    {
        var info = GetTableInfo<T>();
        return updateable.AS($"{info.Item1}.{info.Item2}");
    }

    /// <summary>
    /// 多库新增
    /// </summary>
    /// <param name="insertable"></param>
    /// <returns> </returns>
    public static IInsertable<T> AS<T>(this IInsertable<T> insertable) where T : EntityBase, new()
    {
        var info = GetTableInfo<T>();
        return insertable.AS($"{info.Item1}.{info.Item2}");
    }

    /// <summary>
    /// 多库删除
    /// </summary>
    /// <param name="deleteable"></param>
    /// <returns> </returns>
    public static IDeleteable<T> AS<T>(this IDeleteable<T> deleteable) where T : EntityBase, new()
    {
        var info = GetTableInfo<T>();
        return deleteable.AS($"{info.Item1}.{info.Item2}");
    }

    /// <summary>
    /// 根据实体类型获取表信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static Tuple<string?, string?> GetTableInfo<T>()
    {
        var entityType = typeof(T);
        var attr = entityType.GetCustomAttribute<TenantAttribute>();
        var configId = attr == null ? ConnectionConfigs?.FirstOrDefault()?.ConfigId.ToString() : attr.configId.ToString();
        var tableName = entityType.GetCustomAttribute<SugarTable>()?.TableName;
        return new Tuple<string?, string?>(configId, tableName);
    }
}