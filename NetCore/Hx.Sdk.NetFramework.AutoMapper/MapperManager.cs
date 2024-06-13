using AutoMapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.AutoMapper
{
    /// <summary>
    /// 数据模型映射帮助方法，使用之前先进行初始化
    /// </summary>
    public class MapperManager
    {
        private static IMapper _mapper;
        private static readonly object lockObj = new object();
        /// <summary>
        /// 连接缓存集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, IMapper> MapCache = new ConcurrentDictionary<string, IMapper>();
        /// <summary>
        /// 初始化Web中要使用的Automapper，默认会自动扫描全部程序集，来进行映射
        /// 实现了IAutoMapper接口的类
        /// </summary>
        public static void InitWebMapper(Action<IMapperConfigurationExpression> config = null)
        {
            var key = "webmapper";
            MapCache.TryGetValue(key, out _mapper);
            if (_mapper == null)
            {
                lock (lockObj)
                {
                    if (_mapper == null)
                    {
                        var mapperConfig = new MapperConfiguration(cfg => {
                            cfg.AddProfile<MyMapperProfile>();
                            config?.Invoke(cfg);
                        });
                        _mapper = mapperConfig.CreateMapper();
                        MapCache.TryAdd(key, _mapper);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化automapper
        /// </summary>
        public static void InitMapper(Action<IMapperConfigurationExpression> config)
        {
            var key = "mapper";
            MapCache.TryGetValue(key, out _mapper);
            if (_mapper == null)
            {
                lock (lockObj)
                {
                    if (_mapper == null)
                    {
                        var mapperConfig = new MapperConfiguration(cfg => {
                            config?.Invoke(cfg);
                        });
                        _mapper = mapperConfig.CreateMapper();
                        MapCache.TryAdd(key, _mapper);
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
            if (_mapper == null) throw new Exception("请先初始化Mapper");
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
            if (_mapper == null) throw new Exception("请先初始化Mapper");
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}
