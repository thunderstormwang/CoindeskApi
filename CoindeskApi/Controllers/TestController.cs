using Microsoft.AspNetCore.Mvc;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("hello")]
    public IActionResult GetTodoItem()
    {
        return Ok("hello world!!");
    }
}