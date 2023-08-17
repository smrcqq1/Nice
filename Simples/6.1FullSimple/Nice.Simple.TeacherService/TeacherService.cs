#region using
using Nice.Services;
using Nice.Simple.Contracts;
using Nice.Simple.Contracts.DTO;
using Nice.Simple.Contracts.Enums;
#endregion using
namespace Nice.Simple.Service
{
    public class TeacherService :ServiceBase<DataModel.Teacher>, ITeacherAPI
    {
        public TeacherService(IDataProvider dataProvider):base(dataProvider)
        {
        }
        public Task<List<Teacher>> ListAsync()
        {
            return ReadOnlySet
                .ToListAsync(o=>new Teacher() { 
                    ID = o.Id,
                    Name = o.Name
                });
        }
        public Task<Guid> AddAsync()
        {
            return Set.AddAsync(new DataModel.Teacher()
                {
                    Name = "新教师",
                    Status = TeacherStatus.在职
                });
        }

        public Task<TeacherStatus?> CheckStatusAsync(Guid id)
        {
            return ReadOnlySet.SingleOrDefaultAsync(id,o=>o.Status);
        }
    }
}
