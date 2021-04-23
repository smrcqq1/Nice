using Nice.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Contracts;
using Test.Database.Entities;

namespace Test.Services
{
    public class StudentService : IStudentService
    {
        private readonly Nice.IORM Orm;
        public StudentService(Nice.IORM orm)
        {
            Orm = orm;
        }

        public Task<bool> Add(AddRequest request)
        {
            return Orm.Set<Student>()
                .AddAsync(new Student()
                {
                    Name = request.Name,
                    TeacherID = request.TeacherID
                });
        }

        public Task<bool> Edit(NamedItem request)
        {
            return Orm.Set<Student>()
                .UpdateAsync(request.ID
                , o => new Student()
                {
                    Name = request.Name
                });
        }

        public async Task<InfoRequest> Get(int id)
        {
            var res = await Orm.ReadOnlySet<Grade>()
                .ToListAsync(o => new NamedItem()
                {
                    ID = o.ID
                ,
                    Name = o.Name
                });
            return await Orm.ReadOnlySet<Student>()
                .SingleOrDefaultAsync(id
                , o => new InfoRequest()
                {
                    Name = o.Name,
                    Grade = res[0].Name,
                    Teacher = o.Teacher.Name
                });
        }

        public Task<List<NamedItem>> List()
        {
            return Orm.ReadOnlySet<Student>()
                .ToListAsync(o => new NamedItem()
                {
                    ID = o.ID,
                    Name = o.Name
                });
        }
    }
}