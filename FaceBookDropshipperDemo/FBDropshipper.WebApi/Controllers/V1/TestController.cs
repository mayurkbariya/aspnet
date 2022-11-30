using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    public class TestController : BaseController
    {
        [HttpGet]
        public IActionResult TestError()
        {
            throw new Exception("A Test Exception");
        }
    }
}
