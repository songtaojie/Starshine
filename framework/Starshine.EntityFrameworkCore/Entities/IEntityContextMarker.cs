// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 标记实体
/// </summary>
/// <typeparam name="TContext"></typeparam>
public interface IEntityContextMarker<TContext> where TContext : DbContext
{
    // 这个接口可能不需要任何成员，它只是一个标记  
}
