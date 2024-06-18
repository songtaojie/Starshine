using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 如果没有提供对应模型的服务类，可以使用该方法进行CRUD操作
    /// </summary>
    internal class DbFactory : IDbFactory
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">服务的实例</param>
        public DbFactory( DbContext db)
        {
            this.Db = db;
        }
        /// <summary>
        /// ef数据库上下文
        /// </summary>
        public DbContext Db
        {
            get;
        }
       
        /// <summary>
        /// 根据id获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public Task<T> FindAsync<T>(params object[] keyValues) where T : class
        {
            return this.Db.FindAsync<T>(keyValues).AsTask();
        }
        /// <summary>
        /// 获取满足条件的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return this.Db.Set<T>().FirstOrDefaultAsync(predicate);
        }
        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        public IQueryable<T> QueryEntities<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return this.Db.Set<T>().Where(predicate);
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="handler"></param>
        public async Task<bool> ExcuteAsync(EventHandler handler)
        {
            using IDbContextTransaction transaction = await Db.Database.BeginTransactionAsync();
            try
            {
                handler?.Invoke(null, EventArgs.Empty);
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw new System.Reflection.TargetInvocationException(e);
            }
        }
        /// <summary>
        /// 判断是否存在满足条件的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<bool> ExistAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return this.Db.Set<T>().AnyAsync(predicate);
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            var result = await this.Db.SaveChangesAsync();
            return result > 0;
        }

        #region 新增
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        public async Task<T> InsertAsync<T>(T entity) where T : class, new()
        {
            var result = await this.Db.Set<T>().AddAsync(entity);
            return result.Entity;
        }
        /// <summary>
        /// 插入集合
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public async Task BatchInsertAsync<T>(IEnumerable<T> entityList) where T : class, new()
        {
            await this.Db.Set<T>().AddRangeAsync(entityList);
        }
        #endregion

        #region 更新

        /// <inheritdoc cref="IDbFactory.UpdateAsync{T}(T)"/>
        public async Task<T> UpdateAsync<T>(T entity) where T : class, new()
        {
            return await Task.Run(() =>
            {
                var result = Db.Set<T>().Update(entity);
                return result.Entity;
            });
        }

        /// <inheritdoc cref="IDbFactory.BatchUpdateAsync{T}(T[])"/>
        public async Task BatchUpdateAsync<T>(params T[] entitys) where T : class, new()
        {
            if (entitys != null && entitys.Length > 0)
            {
                await Task.Run(() =>
                {
                    Db.Set<T>().UpdateRange(entitys);
                });
            }
        }

        /// <inheritdoc cref="IDbFactory.UpdatePartialAsync{T}(T, string[])"/>
        public async Task UpdatePartialAsync<T>(T entity, params string[] fields) where T : class, new()
        {
            if (entity != null && fields != null)
            {
                await Task.Run(() =>
                {
                    this.Db.Set<T>().Attach(entity);
                    foreach (var item in fields)
                    {
                        this.Db.Entry<T>(entity).Property(item).IsModified = true;
                    }
                });
            }
        }
        #endregion

        #region 删除
        /// <inheritdoc cref="IDbFactory.RemoveAsync{T}(T)"/>
        public async Task<T> RemoveAsync<T>(T entity) where T : class, new()
        {
            return await Task.Run(() =>
            {
                return this.Db.Remove(entity).Entity;
            });
        }

        /// <inheritdoc cref="IDbFactory.BatchRemoveAsync{T}(IEnumerable{T})"/>
        public async Task<bool> BatchRemoveAsync<T>(IEnumerable<T> entitys) where T : class, new()
        {
            return await Task.Run(() =>
            {
                this.Db.RemoveRange(entitys);
                return true;
            });
        }
        #endregion
    }
}
