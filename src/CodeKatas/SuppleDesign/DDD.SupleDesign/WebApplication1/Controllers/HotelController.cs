using Microsoft.AspNetCore.Mvc;

namespace DDD.SuppleDesign.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        //http post=> Hotel
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] AddHotelCommand command)
        {
            return Ok();
        }

        // http post => api/Hotel/{#id}/AddRoom
        [HttpPost("{id}/addroom")]
        public async Task<IActionResult> AddRoom([FromBody] AddHotelCommand command)
        {
            return Ok();
        }
    }
}