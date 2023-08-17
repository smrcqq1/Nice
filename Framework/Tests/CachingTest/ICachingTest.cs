namespace CachingTest
{
    public interface ICachingTestAPI:Nice.IAPI
    {
        Task EditStudentInfo(EditStudentRequest request);
        Task<string> GetStudentInfo(Guid id);
    }
    public class EditStudentRequest: IIDItem
    {
        public Guid ID { get; set; }
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public string Name { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    }

    public interface IIDItem
    {
        public Guid ID { get; set; }
    }
}
