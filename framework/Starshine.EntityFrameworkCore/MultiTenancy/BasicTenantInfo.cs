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
///// 租户信息
///// </summary>
///// <typeparam name="TKey"></typeparam>
//public class BasicTenantInfo<TKey>
//{
//    /// <summary>
//    /// 租户id，非租户系统为null
//    /// </summary>
//    public TKey? TenantId { get; }

//    /// <summary>
//    /// 租户名字
//    /// </summary>
//    public string? Name { get; }

//    /// <summary>
//    /// 租户信息
//    /// </summary>
//    /// <param name="tenantId"></param>
//    /// <param name="name"></param>
//    public BasicTenantInfo(TKey? tenantId, string? name = null)
//    {
//        TenantId = tenantId;
//        Name = name;
//    }
//}
