using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库上下文操作类
    /// </summary>
    public interface IDbFactory
    {
        /// <summary>
        /// 服务的实例
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetRequiredService<T>();

        /// <summary>
        ///  根据Id获取实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="keyValues">实体的ID</param>
        /// <returns></returns>
        Task<T> FindAsync<T>(params object[] keyValues) where T : class;

        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        IQueryable<T> QueryEntities<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// 判断是否存在满足条件的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> ExistAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="handler"></param>
        Task<bool> ExcuteAsync(EventHandler handler);

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();

        #region 新增
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        Task<T> InsertAsync<T>(T entity) where T : class, new();
        /// <summary>
        /// 插入集合
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        Task BatchInsertAsync<T>(IEnumerable<T> entityList) where T : class, new();
        #endregion

        #region 更新
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">要更新的实体对象</param>
        /// <returns></returns>
        Task<T> UpdateAsync<T>(T entity) where T : class, new();

        /// <summary>
        /// 更新实体的部分字段
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="fields">要更新的字段的集合</param>
        Task UpdatePartialAsync<T>(T entity, params string[] fields) where T : class, new();

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        Task BatchUpdateAsync<T>(params T[] entitys) where T : class, new();
        #endregion

        #region 删除
        /// <summary>
        /// 删除某个实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns></returns>
        Task<T> RemoveAsync<T>(T entity) where T : class, new();

        /// <summary>
        ///批量删除实体
        /// </summary>
        /// <param name="entitys">要删除的实体集合</param>
        /// <returns></returns>
        Task<bool> BatchRemoveAsync<T>(IEnumerable<T> entitys) where T : class, new();
        #endregion

    }
}
