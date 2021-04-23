using Microsoft.AspNetCore.Mvc;
using Nice.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Contracts;

namespace Test.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController: ControllerBase,Test.Contracts.IStudentService
    {
        private readonly Test.Contracts.IStudentService StudentService;
        public StudentController(Test.Contracts.IStudentService studentService)
        {
            StudentService = studentService;
        }

        [HttpPost]
        [Route("Add")]
        public Task<bool> Add(AddRequest request)
        {
            return StudentService.Add(request);
        }

        [HttpPost]
        [Route("Edit")]
        public Task<bool> Edit(NamedItem request)
        {
            return StudentService.Edit(request);
        }

        [HttpGet]
        [Route("Get")]
        //[Cache]
        public Task<InfoRequest> Get(int id)
        {
            return StudentService.Get(id);
        }

        [HttpGet]
        [Route("List")]
        [ResponseCache(Duration = 1200,Location = ResponseCacheLocation.Any)]
        //[Cache]
        public Task<List<NamedItem>> List()
        {
            return StudentService.List();
        }
    }
}