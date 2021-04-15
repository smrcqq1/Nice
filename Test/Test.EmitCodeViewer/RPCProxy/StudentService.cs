using System.Threading.Tasks;
using Test.Contracts;

namespace Test
{
    public class StudentService : Contracts.IStudentService
    {
        private readonly Nice.IRPC rpc;
        public StudentService(Nice.IRPC rpc)
        {
            this.rpc = rpc;
        }
        public Task<bool> Edit(NamedItem request)
        {
            return rpc.Post<bool>(rpc.BaseURL,request);
        }

        public Task<NamedItem> Get(int id)
        {
            return rpc.Get<NamedItem>(rpc.BaseURL + "?id=" + id);
        }
    }
}