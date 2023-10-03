using Microsoft.AspNetCore.Mvc;

namespace DDD.SuppleDesign.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddHotelCommand command)
        {
            return Ok();
        }
    }
}