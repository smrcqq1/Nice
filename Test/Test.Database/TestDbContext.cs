using Microsoft.EntityFrameworkCore;
using Test.Database.Entities;

namespace Test.Database
{
    public class TestDbContext:DbContext
    {
        public TestDbContext(DbContextOptions options) :base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Student> Students { get; set; }
    }
}
