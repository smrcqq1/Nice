using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.MangoDB
{
    public static class Extentions
    {
        public static IServiceCollection UseMongoDB(this IServiceCollection services)
        {
            return services;
        }
        public static void Test()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test");
            var table = database.GetCollection<Test.Database.Entities.Teacher>(nameof(Database.Entities.Teacher));
            table.InsertOne(new Database.Entities.Teacher()
            {
                ID = 2,
                Name = "老师2",
                OperationRecords = new List<Nice.Entities.DetailOperationRecord>() {
                new Nice.Entities.DetailOperationRecord(){
                    ID = 1,
                    RecordID = 1,
                    Time = DateTime.Now
                }
            }});
            var list = table.AsQueryable().ToList();
        }
    }
}