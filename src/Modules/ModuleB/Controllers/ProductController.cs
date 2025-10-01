using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ModuleB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("hello 123");
        }
    }
}
