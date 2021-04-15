using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Contracts
{
    public interface IStudentService
    {
        Task<Nice.DTO.NamedItem> Get(int id);
        Task<bool> Edit(Nice.DTO.NamedItem request);
    }
}
