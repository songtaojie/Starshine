using Starshine.DatabaseAccessor.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hx.Sdk.Extensions
{
    /// <summary>
    /// 数据库数据转换拓展
    /// </summary>
    public static class DbDataConvertExtensions
    {
        /// <summary>
        /// 将 DataTable 转 List 集合
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="dataTable">DataTable</param>
        /// <returns>List{T}</returns>
        public static List<T> ToList<T>(this DataTable dataTable)
        {
            return dataTable.ToList(typeof(List<T>)) as List<T>;
        }

        /// <summary>
        /// 将 DataTable 转 List 集合
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="dataTable">DataTable</param>
        /// <returns>List{T}</returns>
        public static async Task<List<T>> ToListAsync<T>(this DataTable dataTable)
        {
            var list = await dataTable.ToListAsync(typeof(List<T>));
            return list as List<T>;
        }

        /// <summary>
        /// 将 DataSet 转 元组
        /// </summary>
        /// <typeparam name="T1">元组元素类型</typeparam>
        /// <param name="dataSet">DataSet</param>
        /// <returns>元组类型</returns>
        public static List<T1> ToList<T1>(this DataSet dataSet)
        {
            var tuple = dataSet.ToList(typeof(List<T1>));
            return tuple[0] as List<T1>;
        }

        /// <summary>
        /// 将 DataSet 转 特定类型
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="returnType">特定类型集合</param>
        /// <returns>List{object}</returns>
        public static List<object> ToList(this DataSet dataSet, Type returnType)
        {
            if (returnType == null) return default;

            // 处理元组类型

            if (returnType.IsValueType)
            {
                returnType = returnType.GenericTypeArguments.FirstOrDefault();
            }

            // 获取所有的 DataTable
            var dataTables = dataSet.Tables;

            return new List<object>
            {
                dataTables[0].ToList(returnType)
            };
        }

        /// <summary>
        /// 将 DataSet 转 特定类型
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="returnType">特定类型集合</param>
        /// <returns>object</returns>
        public static Task<List<object>> ToListAsync(this DataSet dataSet, Type returnType)
        {
            return Task.FromResult(dataSet.ToList(returnType));
        }

        /// <summary>
        /// 将 DataTable 转 特定类型
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="returnType">返回值类型</param>
        /// <returns>object</returns>
        public static object ToList(this DataTable dataTable, Type returnType)
        {
            var isGenericType = returnType.IsGenericType;
            // 获取类型真实返回类型
            var underlyingType = isGenericType ? returnType.GenericTypeArguments.First() : returnType;

            var resultType = typeof(List<>).MakeGenericType(underlyingType);
            var list = Activator.CreateInstance(resultType);
            var addMethod = resultType.GetMethod("Add");

            // 将 DataTable 转为行集合
            var dataRows = dataTable.AsEnumerable();

            // 如果是基元类型
            if (underlyingType.IsRichPrimitive())
            {
                // 遍历所有行
                foreach (var dataRow in dataRows)
                {
                    // 只取第一列数据
                    var firstColumnValue = dataRow[0];
                    // 转换成目标类型数据
                    var destValue = firstColumnValue?.ChangeType(underlyingType);
                    // 添加到集合中
                    _ = addMethod.Invoke(list, new[] { destValue });
                }
            }
            // 处理Object类型
            else if (underlyingType == typeof(object))
            {
                // 获取所有列名
                var columns = dataTable.Columns;

                // 遍历所有行
                foreach (var dataRow in dataRows)
                {
                    var dic = new Dictionary<string, object>();
                    foreach (DataColumn column in columns)
                    {
                        dic.Add(column.ColumnName, dataRow[column]);
                    }
                    _ = addMethod.Invoke(list, new[] { dic });
                }
            }
            else
            {
                // 获取所有的数据列和类公开实例属性
                var dataColumns = dataTable.Columns;
                var properties = underlyingType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                //.Where(p => !p.IsDefined(typeof(NotMappedAttribute), true));  // sql 数据转换无需判断 [NotMapperd] 特性

                // 遍历所有行
                foreach (var dataRow in dataRows)
                {
                    var model = Activator.CreateInstance(underlyingType);

                    // 遍历所有属性并一一赋值
                    foreach (var property in properties)
                    {
                        // 获取属性对应的真实列名
                        var columnName = property.Name;
                        if (property.IsDefined(typeof(ColumnAttribute), true))
                        {
                            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>(true);
                            if (!string.IsNullOrWhiteSpace(columnAttribute.Name)) columnName = columnAttribute.Name;
                        }

                        // 如果 DataTable 不包含该列名，则跳过
                        if (!dataColumns.Contains(columnName)) continue;

                        // 获取列值
                        var columnValue = dataRow[columnName];
                        // 如果列值未空，则跳过
                        if (columnValue == DBNull.Value) continue;

                        // 转换成目标类型数据
                        var destValue = columnValue?.ChangeType(property.PropertyType);
                        property.SetValue(model, destValue);
                    }

                    // 添加到集合中
                    _ = addMethod.Invoke(list, new[] { model });
                }
            }

            return list;
        }

        /// <summary>
        /// 将 DataTable 转 特定类型
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="returnType">返回值类型</param>
        /// <returns>object</returns>
        public static Task<object> ToListAsync(this DataTable dataTable, Type returnType)
        {
            return Task.FromResult(dataTable.ToList(returnType));
        }

    }
}