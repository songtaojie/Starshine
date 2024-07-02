using Microsoft.EntityFrameworkCore;
using Starshine.EntityFrameworkCore;

namespace Starshine.TestProject
{
    [StarshineDbContext("")]
    public class TestDbContext: StarshineDbContext<TestDbContext>
    {
        public TestDbContext(DbContextOptions<TestDbContext> options):base(options)
        { 
            
        }
    }
}
