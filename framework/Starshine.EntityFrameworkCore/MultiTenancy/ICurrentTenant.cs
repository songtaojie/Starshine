//// MIT License
////
//// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
////
//// 电话/微信：song977601042

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Starshine.EntityFrameworkCore;
///// <summary>
///// 当前租户信息
///// </summary>
///// <typeparam name="TKey"></typeparam>
//public interface ICurrentTenant<TKey>
//{
//    /// <summary>
//    /// 是可用的
//    /// </summary>
//    bool IsAvailable { get; }

//    /// <summary>
//    /// 租户id
//    /// </summary>
//    TKey? Id { get; }

//    /// <summary>
//    /// 租户名称
//    /// </summary>
//    string? Name { get; }

//    /// <summary>
//    /// 改变当前租户
//    /// </summary>
//    /// <param name="id"></param>
//    /// <param name="name"></param>
//    /// <returns></returns>
//    void Change(TKey? id, string? name = null);
//}
