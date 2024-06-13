using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Sqlsugar.Repositories
{
    /// <summary>
    /// SqlSugar 仓储实现类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class SqlSugarRepository<TEntity> : SimpleClient<TEntity>, ISqlSugarRepository<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 非泛型 SqlSugar 仓储
        /// </summary>

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public SqlSugarRepository(ISqlSugarClient db)
        {
            Context = db;
            // 若实体贴有多库特性，则返回指定库连接
            if (typeof(TEntity).IsDefined(typeof(TenantAttribute), false))
            {
                Context = db.AsTenant().GetConnectionScopeWithAttr<TEntity>();
            }
        }

        /// <summary>
        /// 实体集合
        /// </summary>
        public virtual ISugarQueryable<TEntity> Entities => Context.Queryable<TEntity>();

        /// <summary>
        /// 原生 Ado 对象
        /// </summary>
        public IAdo Ado => Context.Ado;
    }
}
