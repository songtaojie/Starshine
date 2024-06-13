using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hx.Sdk.NetFramework.AutoMapper
{
    /// <summary>
    /// 个人的一个配置，用来配置映射的规则，
    /// </summary>
    internal class MyMapperProfile : Profile
    {
        public MyMapperProfile()
        {
            List<Assembly> allAssembly = RuntimeHelper.GetAllAssembly();
            if (allAssembly != null && allAssembly.Count > 0)
            {
                Type baseType = typeof(DatabaseAccessor.IAutoMapper<>);
                foreach (Assembly ass in allAssembly)
                {
                    try
                    {
                        var types = ass.GetExportedTypes().Where(t => !t.IsInterface && !t.IsAbstract);
                        foreach (Type sourceType in types)
                        {
                            var genericTypes = sourceType.GetGenericInterfaces(baseType);
                            if (genericTypes != null && genericTypes.Count() > 0)
                            {
                                foreach (Type t in genericTypes)
                                {
                                    Type destType = t.GetGenericElementType();
                                    if (destType.IsClass)
                                    {
                                        CreateMap(sourceType, destType);
                                        CreateMap(destType, sourceType);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

    }
}
