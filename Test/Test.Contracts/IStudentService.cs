using Nice.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Contracts
{
    public interface IStudentService
    {
        Task<List<NamedItem>> List();
        Task<InfoRequest> Get(int id);
        Task<bool> Edit(Nice.DTO.NamedItem request);
        Task<bool> Add(AddRequest request);
    }
    public class AddRequest
    {
        [Required]
        [StringLength(20,MinimumLength = 2)]
        public string Name { get; set; }
        public int TeacherID { get; set; }
    }
    public class InfoRequest
    {
        public string Name { get; set; }
        public string Teacher { get; set; }
        public string Grade { get; set; }
    }
}