using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Mapper
{
    /// <summary>
    /// 数据模型映射帮助方法
    /// </summary>
    public class MapperManager
    {
        /// <summary>
        /// 一个委托在Build之前会执行当前委托
        /// </summary>
        public static event Action<IMapperConfigurationExpression> Config;
        private static IMapper _mapper;
        private static readonly object lockObj = new object();
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            if (_mapper == null)
            {
                lock (lockObj)
                {
                    if (_mapper == null)
                    {
                        var config = new MapperConfiguration(cfg => {
                            cfg.AddProfile<MyMapperProfile>();
                            Config?.Invoke(cfg);
                        });
                        _mapper = config.CreateMapper();
                    }
                }
            }
        }

        /// <summary>
        /// 数据映射
        /// </summary>
        /// <typeparam name="TDestination">目标数据类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static TDestination Map<TDestination>(object source)
        {
            Init();
            return _mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 映射数据
        /// </summary>
        /// <typeparam name="TSource">元数据类型</typeparam>
        /// <typeparam name="TDestination">目标数据类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            Init();
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}
