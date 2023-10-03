using Microsoft.AspNetCore.Mvc;

namespace DDD.SuppleDesign.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddRoomCommand command)
        {
            return Ok();
        }
    }
}