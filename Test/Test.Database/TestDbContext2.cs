using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Database.Entities;

namespace Test.Database
{
    public class TestDbContext2 : DbContext
    {
        public TestDbContext2(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public virtual DbSet<Grade> Grades { get; set; }
    }
}
