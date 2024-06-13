using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 类型扩展函数
    /// </summary>
    public static class TypeExtensions
    {
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

        /// <summary>
        /// 确定是否可以将指定类型的实例分配给当前泛型类型的变量。
        /// </summary>
        /// <param name="genericType">当前泛型类型</param>
        /// <param name="givenType">指定类型</param>
        /// <returns></returns>
        public static bool IsAssignableFromGenericType(this Type genericType, Type givenType)
        {
            if (givenType.IsGenericType(genericType)) return true;
            var interfaceTypes = givenType.GetInterfaces();
            foreach (var it in interfaceTypes)
            {

                if (it.IsGenericType(genericType))
                    return true;
            }
            Type baseType = givenType.BaseType;
            if (baseType == null) return false;
            return genericType.IsAssignableFromGenericType(baseType);
        }
    }
}
