using Starshine.DependencyInjection;
using Starshine.EntityFrameworkCore;
using Starshine.TestProject.Entities;

namespace Starshine.TestProject.Repositorys
{
    public class TestEfCoreRepository : EFCoreRepository<TestDbContext, TestEntity>,ITransientDependency
    {
        public TestEfCoreRepository(IServiceProvider scoped) : base(scoped)
        {
        }
    }
}
