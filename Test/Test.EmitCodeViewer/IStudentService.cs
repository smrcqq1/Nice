using System.Threading.Tasks;

namespace Test.Contracts
{
    public interface IStudentService
    {
        Task<NamedItem> Get(int id);
        Task<bool> Edit(NamedItem request);
    }
    public class NamedItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
