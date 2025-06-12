using eRekreacijaAPI.DTOs;
using eRekreacijaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        [HttpPost("AddHoliday")]
        public async Task<IActionResult> AddHoliday([FromBody] HolidayDTO holiday)
        {
            var result = await _holidayService.AddHolidayAsync(holiday);
            return Ok(result);
        }

        [HttpPost("AddObjectHoliday")]
        public async Task<IActionResult> AddObjectHoliday([FromBody] ObjectHolidayDTO dto)
        {
            var result = await _holidayService.AddObjectHolidayAsync(dto);
            if (result == null)
                return BadRequest("Holiday already assigned to this object.");
            return Ok(result);
        }
    }
}
