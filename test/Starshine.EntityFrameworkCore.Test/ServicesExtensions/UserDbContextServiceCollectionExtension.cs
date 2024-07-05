using Microsoft.Extensions.DependencyInjection;
using Starshine.EntityFrameworkCore;
using Starshine.EntityFrameworkCore.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UserDbContextServiceCollectionExtension
    {
        public static IServiceCollection AddUserDbContextService(this IServiceCollection services)
        {
            services.AddEntityFrameworkMySql();
            services.AddStarshineEfCore()
                .AddStarshineDbContextPool<UserDbContext>();
            services.AddTransient<IModelBuilderFilter<UserDbContext>, UserDbContextModelBuilderFilter>();
            return services;
        }
    }
}
