// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Sqlsugar;
/// <summary>
/// 日志作用域
/// </summary>
public class SugarLogScope
{
    /// <summary>
    /// sql来源
    /// </summary>
    public const string Source = "SugarSource";

    /// <summary>
    /// sql来源
    /// </summary>
    internal const string SourceValue = "Starshine.Sqlsugar";

    /// <summary>
    /// sql语句
    /// </summary>
    public const string Sql = "SugarSql";

    /// <summary>
    /// sql语句参数
    /// </summary>
    public const string SqlPars = "SugarSqlPars";

    /// <summary>
    /// sql行为
    /// </summary>
    public const string SugarActionType = "SugarActionType";

    /// <summary>
    /// 日志类型，1：普通Sql日志，2：异常Sql日志，3：差异化日志
    /// </summary>
    public const string LogType = "SugarLogType";
}
