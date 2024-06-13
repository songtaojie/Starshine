﻿using System;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 禁止规范化处理
    /// </summary>
    [SkipScan, AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class NonUnifyAttribute : Attribute
    {
    }
}