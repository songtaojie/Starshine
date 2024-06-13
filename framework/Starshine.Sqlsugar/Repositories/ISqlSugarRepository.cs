using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sqlsugar
{

    /// <summary>
    /// SqlSugar 仓储接口定义
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial interface ISqlSugarRepository<TEntity>: ISugarRepository, ISimpleClient<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 实体集合
        /// </summary>
        ISugarQueryable<TEntity> Entities { get; }

        /// <summary>
        /// 原生 Ado 对象
        /// </summary>
        IAdo Ado { get; }
    }
}
