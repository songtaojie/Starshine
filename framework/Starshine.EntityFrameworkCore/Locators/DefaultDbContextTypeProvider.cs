using Microsoft.Extensions.Options;
using Starshine.DependencyInjection;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 默认数据库上下文定位器
    /// </summary>
    public class DefaultDbContextTypeProvider : IDbContextTypeProvider, IScopedDependency
    {
        private readonly StarshineDbContextTypeOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public DefaultDbContextTypeProvider(IOptions<StarshineDbContextTypeOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        public virtual Type GetDbContextType(Type dbContextType)
        {
            return _options.GetReplacedTypeOrSelf(dbContextType);
        }
    }
}