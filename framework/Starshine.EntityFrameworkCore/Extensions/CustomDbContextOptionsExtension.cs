using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 自定义DBContext选项
    /// </summary>
    internal class CustomDbContextOptionsExtension : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo _info;
        /// <summary>
        /// 配置服务选项
        /// </summary>
        /// <param name="services"></param>
        public void ApplyServices(IServiceCollection services)
        {
            //Console.WriteLine("ApplyServices");
            services.AddSingleton<IConventionSetPlugin, CustomConventionSetPlugin>();
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="options"></param>
        public void Validate(IDbContextOptions options) { }

        /// <summary>
        /// 
        /// </summary>
        public DbContextOptionsExtensionInfo Info
        {
            get
            {
                DbContextOptionsExtensionInfo result;
                if ((result = this._info) == null)
                {
                    result = (this._info = new CustomDbContextOptionsExtension.ExtensionInfo(this));
                }
                return result;
            }
        }

        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            private string _logFragment;

            public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
            {
            }

            private new CustomDbContextOptionsExtension Extension
            {
                get
                {
                    return (CustomDbContextOptionsExtension)base.Extension;
                }
            }

            public override string LogFragment
            {
                get
                {
                    if (this._logFragment == null)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(nameof(CustomDbContextOptionsExtension));
                        this._logFragment = stringBuilder.ToString();
                    }
                    return this._logFragment;
                }
            }

            public override bool IsDatabaseProvider => false;

            public override long GetServiceProviderHashCode() => 0;

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
                debugInfo[nameof(CustomDbContextOptionsExtension)] = "1";
            }
        }
    }
}
