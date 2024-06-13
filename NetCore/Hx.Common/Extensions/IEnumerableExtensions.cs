﻿using Hx.Common.Extensions.LinqBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Hx.Common;
namespace Hx.Common.Extensions
{
    /// <summary>
    /// IEnumerable 拓展
    /// </summary>
    [SkipScan]
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// [EF Core] 根据条件成立再构建 Where 查询
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">表达式</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> WhereIF<TSource>(this IQueryable<TSource> sources, bool condition, Expression<Func<TSource, bool>> expression)
        {
            return condition ? Queryable.Where(sources, expression) : sources;
        }

        /// <summary>
        /// [EF Core] 根据条件成立再构建 Where 查询，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">表达式</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> WhereIF<TSource>(this IQueryable<TSource> sources, bool condition, Expression<Func<TSource, int, bool>> expression)
        {
            return condition ? Queryable.Where(sources, expression) : sources;
        }

        /// <summary>
        /// [EF Core] 与操作合并多个表达式
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="expressions">表达式数组</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> sources, params Expression<Func<TSource, bool>>[] expressions)
        {
            if (expressions == null || !expressions.Any()) return sources;
            if (expressions.Length == 1) return Queryable.Where(sources, expressions[0]);

            var expression = LinqExpression.Or<TSource>();
            foreach (var _expression in expressions)
            {
                expression = expression.Or(_expression);
            }
            return Queryable.Where(sources, expression);
        }

        /// <summary>
        /// [EF Core] 与操作合并多个表达式，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="expressions">表达式数组</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> sources, params Expression<Func<TSource, int, bool>>[] expressions)
        {
            if (expressions == null || !expressions.Any()) return sources;
            if (expressions.Length == 1) return Queryable.Where(sources, expressions[0]);

            var expression = LinqExpression.IndexOr<TSource>();
            foreach (var _expression in expressions)
            {
                expression = expression.Or(_expression);
            }
            return Queryable.Where(sources, expression);
        }

        /// <summary>
        /// [EF Core] 根据条件成立再构建 WhereOr 查询
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="conditionExpressions">条件表达式</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> WhereIF<TSource>(this IQueryable<TSource> sources, params (bool condition, Expression<Func<TSource, bool>> expression)[] conditionExpressions)
        {
            var expressions = new List<Expression<Func<TSource, bool>>>();
            foreach (var (condition, expression) in conditionExpressions)
            {
                if (condition) expressions.Add(expression);
            }
            return Where(sources, expressions.ToArray());
        }

        /// <summary>
        /// [EF Core] 根据条件成立再构建 WhereOr 查询，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="conditionExpressions">条件表达式</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> WhereIF<TSource>(this IQueryable<TSource> sources, params (bool condition, Expression<Func<TSource, int, bool>> expression)[] conditionExpressions)
        {
            var expressions = new List<Expression<Func<TSource, int, bool>>>();
            foreach (var (condition, expression) in conditionExpressions)
            {
                if (condition) expressions.Add(expression);
            }
            return Where(sources, expressions.ToArray());
        }

        /// <summary>
        /// 根据条件成立再构建 Where 查询
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">表达式</param>
        /// <returns>新的集合对象</returns>
        public static IEnumerable<TSource> WhereIF<TSource>(this IEnumerable<TSource> sources, bool condition, Func<TSource, bool> expression)
        {
            return condition ? sources.Where(expression) : sources;
        }

        /// <summary>
        /// 根据条件成立再构建 Where 查询，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">表达式</param>
        /// <returns>新的集合对象</returns>
        public static IEnumerable<TSource> WhereIF<TSource>(this IEnumerable<TSource> sources, bool condition, Func<TSource, int, bool> expression)
        {
            return condition ? sources.Where(expression) : sources;
        }
    }
}
