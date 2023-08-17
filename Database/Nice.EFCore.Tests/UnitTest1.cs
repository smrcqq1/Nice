namespace Nice.EFCore.Tests;
[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        AutoDbContext.SetTables(typeof(Simple.DataModel.Student));
        AutoDbContext.EnsureCreated = true;
        var builder = new DbContextOptionsBuilder<EFCore.AutoDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var db = new EFCore.AutoDbContext(builder.Options);
        var provider = new EFCore.EFDataProvider<AutoDbContext>(db);

        var teachers = provider.Set<Simple.DataModel.Teacher>();
        var newTeachers = new List<Simple.DataModel.Teacher>();
        var total = 10;
        for (var i = 1; i <= total; i++)
        {
            newTeachers.Add(new Simple.DataModel.Teacher()
            {
                Status = Simple.Contracts.Enums.TeacherStatus.��ְ,
                Name = "��ʦ" + i.ToString(),
            });
        }
        teachers.AddAsync(newTeachers.ToArray()).Wait();
        var cnt = teachers.CountAsync().Result;
        Assert.AreEqual(total, cnt);
        teachers.BulkUpdateAsync(o => { o.Status = Simple.Contracts.Enums.TeacherStatus.����ְ; }).Wait();
        teachers.BulkDeleteAsync().Wait();
        cnt = teachers.CountAsync().Result;
        Assert.AreEqual(0, cnt);
    }

    void Set(Simple.DataModel.Teacher o)
    {
        o.Status = Simple.Contracts.Enums.TeacherStatus.����ְ;
    }
    [TestMethod]
    public void ԭ������ɾ������()
    {
        var builder = new DbContextOptionsBuilder<SimpleDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var db = new SimpleDbContext(builder.Options);
        var teachers = db.Simples;
        var total = 10;
        for (var i = 1; i <= total; i++)
        {
            teachers.Add(new SimpleModel()
            {
                Name = "��ʦ" + i.ToString(),
            });
        }
        db.SaveChanges();
        var cnt = teachers.Count();
        Assert.AreEqual(total, cnt);
        //teachers.ExecuteDelete();
        //cnt = teachers.Count();
        //Assert.AreEqual(0, cnt);

        teachers.ExecuteUpdate(o => o.SetProperty(p=>p.Name,"asfasf"));
    }
}