using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.AutoMapper
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// 得到所有的泛型接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericInterface"></param>
        /// <returns></returns>
        public static List<Type> GetGenericInterfaces(this Type type, Type genericInterface)
        {
            List<Type> typeList = new List<Type>();
            var types = type.GetTypeInfo().ImplementedInterfaces.Where(t => t.IsGenericType(genericInterface));
            if (types != null)
            {
                typeList = types.ToList();
            }
            return typeList;
        }

        /// <summary>
        /// 是否是某个泛型类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsGenericType(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }


        public static Type GetGenericElementType(this Type type)
        {
            if (type.HasElementType)
                return type.GetElementType();
            return type.GetTypeInfo().GenericTypeArguments[0];
        }
    }
}
