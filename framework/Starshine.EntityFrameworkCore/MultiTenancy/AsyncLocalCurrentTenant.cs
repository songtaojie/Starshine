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
//public class AsyncLocalCurrentTenant<TKey> : ICurrentTenant<TKey>
//{
//    public AsyncLocalCurrentTenant()
//    { 
        
//    }
//    private readonly AsyncLocal<BasicTenantInfo<TKey>?> _currentScope;

//    /// <summary>
//    /// 是可用的
//    /// </summary>
//    public bool IsAvailable => Id != null;

//    /// <summary>
//    /// 租户id
//    /// </summary>
//    public TKey? Id => _currentScope.Value == null ? default: _currentScope.Value.TenantId;

//    /// <summary>
//    /// 租户名称
//    /// </summary>
//    public string? Name => _currentScope.Value?.Name;

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="tenantId"></param>
//    /// <param name="name"></param>
//    /// <returns></returns>
//    /// <exception cref="NotImplementedException"></exception>
//    public void Change(TKey? tenantId, string? name = null)
//    {
//        _currentScope.Value = new BasicTenantInfo<TKey>(tenantId, name);
//    }
//}
