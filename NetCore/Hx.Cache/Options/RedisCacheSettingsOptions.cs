// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using FreeRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Cache.Options;
/// <summary>
/// redis缓存配置
/// </summary>
public sealed class RedisCacheSettingsOptions
{
    /// <summary>
    /// 用于连接到Redis的配置。
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Slave连接字符串
    /// </summary>
    public IEnumerable<string> SlaveConnectionStrings { get; set; }
}
