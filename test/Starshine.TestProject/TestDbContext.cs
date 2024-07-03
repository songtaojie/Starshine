using Microsoft.EntityFrameworkCore;
using Starshine.EntityFrameworkCore;
using Starshine.TestProject.Entities;

namespace Starshine.TestProject
{
    [StarshineDbContext("Default")]
    public class TestDbContext: StarshineDbContext<TestDbContext>
    {
        public TestDbContext(DbContextOptions<TestDbContext> options):base(options)
        { 
            
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }
}
