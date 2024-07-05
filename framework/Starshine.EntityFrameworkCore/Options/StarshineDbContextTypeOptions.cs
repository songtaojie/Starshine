// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 
/// </summary>
public class StarshineDbContextTypeOptions
{
    internal Dictionary<string, Type> DbContextReplacements { get; } 
    /// <summary>
    /// 
    /// </summary>
    public StarshineDbContextTypeOptions()
    {
        DbContextReplacements = new Dictionary<string, Type>();
    }

    internal Type GetReplacedTypeOrSelf(Type dbContextType)
    {
        var replacementType = dbContextType;
        while (true)
        {
            var foundType = DbContextReplacements.LastOrDefault(x => x.Key == replacementType.FullName);
            if (!foundType.Equals(default(KeyValuePair<string, Type>)))
            {
                if (foundType.Value == dbContextType)
                {
                    return replacementType;
                }
                replacementType = foundType.Value;
            }
            else
            {
                return replacementType;
            }
        }
    }
}
