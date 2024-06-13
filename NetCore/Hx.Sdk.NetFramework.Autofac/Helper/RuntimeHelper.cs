using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace Hx.Sdk.NetFramework.Autofac
{
    internal static class RuntimeHelper
    {
        internal static IEnumerable<MethodInfo> GetStaticMethods(this Type type)
        {
            return type.GetRuntimeMethods().Where(m => m.IsStatic);
        }
        private static List<Assembly> _allAssembly = null;
        /// <summary>
        /// 获取所有程序集
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetAllAssembly()
        {
            if (_allAssembly == null || _allAssembly.Count == 0)
            {
                _allAssembly = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            }
            return _allAssembly;
        }
    }
}
