using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Starshine.EntityFrameworkCore
{
    internal static class DbHelpers
    {
        /// <summary>
        /// 将模型转为 DbParameter 集合
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <param name="dbCommand">数据库命令对象</param>
        /// <returns></returns>
        internal static DbParameter[] ConvertToDbParameters(object model, DbCommand dbCommand)
        {
            var modelType = model.GetType();

            // 处理字典类型参数
            if (modelType == typeof(Dictionary<string, object>)) return ConvertToDbParameters((Dictionary<string, object>)model, dbCommand);

            var dbParameters = new List<DbParameter>();
            if (model == null || !modelType.IsClass) return dbParameters.ToArray();

            // 获取所有公开实例属性
            var properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length == 0) return dbParameters.ToArray();

            // 遍历所有属性
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(model) ?? DBNull.Value;

                // 创建命令参数
                var dbParameter = dbCommand.CreateParameter();

                // 判断属性是否贴有 [DbParameter] 特性
                if (property.IsDefined(typeof(DbParameterAttribute), true))
                {
                    var dbParameterAttribute = property.GetCustomAttribute<DbParameterAttribute>(true)!;
                    dbParameters.Add(ConfigureDbParameter(property.Name, propertyValue, dbParameterAttribute, dbParameter));
                    continue;
                }

                dbParameter.ParameterName = property.Name;
                dbParameter.Value = propertyValue;
                dbParameters.Add(dbParameter);
            }

            return dbParameters.ToArray();
        }

        /// <summary>
        /// 将字典转换成命令参数
        /// </summary>
        /// <param name="keyValues">字典</param>
        /// <param name="dbCommand">数据库命令对象</param>
        /// <returns></returns>
        internal static DbParameter[] ConvertToDbParameters(Dictionary<string, object> keyValues, DbCommand dbCommand)
        {
            var dbParameters = new List<DbParameter>();
            if (keyValues == null || keyValues.Count == 0) return dbParameters.ToArray();

            foreach (var key in keyValues.Keys)
            {
                var value = keyValues[key] ?? DBNull.Value;

                // 创建命令参数
                var dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = key;
                dbParameter.Value = value;
                dbParameters.Add(dbParameter);
            }

            return dbParameters.ToArray();
        }

        /// <summary>
        /// 配置数据库命令参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="dbParameterAttribute">参数特性</param>
        /// <param name="dbParameter">数据库命令参数</param>
        /// <returns>DbParameter</returns>
        internal static DbParameter ConfigureDbParameter(string name, object value, DbParameterAttribute dbParameterAttribute, DbParameter dbParameter)
        {
            // 设置参数方向
            dbParameter.ParameterName = name;
            dbParameter.Value = value;
            dbParameter.Direction = dbParameterAttribute.Direction;

            // 设置参数数据库类型
            if (dbParameterAttribute.DbType != null)
            {
                var type = dbParameterAttribute.DbType.GetType();
                if (type.IsEnum && typeof(DbType).IsAssignableFrom(type))
                {
                    dbParameter.DbType = (DbType)dbParameterAttribute.DbType;
                }
            }
            // 设置大小，解决NVarchar，Varchar 问题
            if (dbParameterAttribute.Size > 0)
            {
                dbParameter.Size = dbParameterAttribute.Size;
            }

            return dbParameter;
        }

        /// <summary>
        /// 数据没找到异常
        /// </summary>
        /// <returns></returns>
        internal static InvalidOperationException DataNotFoundException()
        {
            return new InvalidOperationException("Sequence contains no elements.");
        }

        /// <summary>
        /// 修正不同数据库命令参数前缀不一致问题
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="parameterName"></param>
        /// <param name="isFixed"></param>
        /// <returns></returns>
        internal static string FixSqlParameterPlaceholder(string providerName, string parameterName, bool isFixed = true)
        {
            var placeholder = !DbProvider.IsDatabaseFor(providerName, DbProvider.Oracle) ? "@" : ":";
            if (parameterName.StartsWith("@") || parameterName.StartsWith(":"))
            {
                parameterName = parameterName[1..];
            }

            return isFixed ? placeholder + parameterName : parameterName;
        }

        /// <summary>
        /// Sql 模板正在表达式
        /// </summary>
        private static readonly Regex SqlTemplateRegex;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DbHelpers()
        {
            SqlTemplateRegex = new Regex(@"\#\((?<path>.*?)\)");
        }
    }
}