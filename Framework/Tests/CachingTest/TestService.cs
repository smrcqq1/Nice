using Nice.DTO;
using System;
using System.Threading.Tasks;

namespace CachingTest
{
    public class TestService : ITestAPI
    {
        public Task<string> Delete(int id)
        {
            return Task.FromResult($"Delete id={id}");
        }
        public Task<string> Get()
        {
            return Task.FromResult("Get");
        }

        public void GetByID(int id)
        {
            throw new Nice.BuzinessException("asfasfasf");
        }

        public Task GetNoThing()
        {
            return Task.CompletedTask;
        }

        public string GetString()
        {
            return "GetString";
        }

        public string GetTwoParameter(int id, string name)
        {
            return $"success，id={id},name={name}";
        }

        public string GetTwoParameterPageRequest(int id, PageRequest request)
        {
            return $"success，id={id},size={request.Size},index={request.Index}";
        }

        public Task<string> PostPageRequest(PageRequest request)
        {
            return Task.FromResult($"size={request.Size},index={request.Index}");
        }

        public Task<string> TestRequest(TestRequest request)
        {
            throw new System.NotImplementedException();
        }

        public void Void()
        {
        }
    }
}
