using Nice.DTO;

namespace CachingTest
{
    public interface ITestAPI : Nice.IAPI
    {

        public string GetString();

        public Task GetNoThing();

        public Task<string> Get();

        Task<string> TestRequest(TestRequest request);

        Task<string> Delete(int id);

        void GetByID(int id);

        string GetTwoParameter(int id, string name);

        string GetTwoParameterPageRequest(int id, PageRequest request);
    }

    public class TestRequest
    {
        public string? Max { get; set; }
    }
}