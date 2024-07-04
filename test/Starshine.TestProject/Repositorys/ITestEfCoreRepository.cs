using Starshine.EntityFrameworkCore;
using Starshine.TestProject.Entities;

namespace Starshine.TestProject.Repositorys
{
    public interface ITestEfCoreRepository:IEFCoreRepository<TestEntity>
    {
    }
}
