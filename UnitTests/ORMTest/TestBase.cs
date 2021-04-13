using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Test.Database.Entities;

namespace ORMTest
{
    public class TestBase : Nice.UnitTestBase
    {
        protected override IServiceCollection Config(IServiceCollection services)
        {
            return services.UseNice()
                .UseEFCore<Test.Database.TestDbContext>(o =>
                {
                    o.UseMySql("data source=localhost;database=testorm; uid=root;pwd=q111111;");
                });
        }
    }
}