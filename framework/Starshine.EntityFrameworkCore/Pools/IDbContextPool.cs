using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库上下文池
    /// </summary>
    public interface IDbContextPool
    {
        /// <summary>
        /// 获取所有数据库上下文
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<DbContext> GetAllActiveDbContexts();

        /// <summary>
        /// 保存数据库上下文（异步）
        /// </summary>
        /// <param name="dbContext"></param>
        Task AddToPoolAsync(DbContext dbContext);

        /// <summary>
        /// 保存数据库上下文池中所有已更改的数据库上下文
        /// </summary>
        /// <returns></returns>
        int SavePoolNow();

        /// <summary>
        /// 保存数据库上下文池中所有已更改的数据库上下文
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SavePoolNowAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 设置数据库上下文共享事务
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        void ShareTransaction(int skipCount, DbTransaction transaction);

        /// <summary>
        /// 设置数据库上下文共享事务
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="transaction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ShareTransactionAsync(int skipCount, DbTransaction transaction, CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RollbackAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 关闭所有数据库链接
        /// </summary>
        void CloseAll();
    }
}