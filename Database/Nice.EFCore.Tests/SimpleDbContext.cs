namespace Nice.EFCore.Tests
{
    public class SimpleDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public SimpleDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        //public DbSet<Nice.Simple.DataModel.Student> Students { get; set; }
        //public DbSet<Nice.Simple.DataModel.Teacher> Teachers { get; set; }
        public DbSet<SimpleModel> Simples { get; set; }
    }
}

public class SimpleModel
{
    public Guid ID { get; set; }
    public string Name { get; set; }
}
