using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Test.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController: ControllerBase
    {
        [HttpGet]
        [Route("List")]
        [Cache]
        public async Task<string> ListAsync(string id)
        {
            return await Task.FromResult(id);
        }
    }
}