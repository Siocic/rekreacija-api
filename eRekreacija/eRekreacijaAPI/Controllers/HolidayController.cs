using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HolidayController : ControllerBase
    {
        private readonly RekreacijaContext _context;

        public HolidayController(RekreacijaContext context)
        {
            _context = context;
        }

        [HttpPost("AddHoliday")]
        public async Task<IActionResult> AddHoliday([FromBody] tbl_Holiday holiday)
        {
            _context.TblHoliday.Add(holiday);
            await _context.SaveChangesAsync();
            return Ok(holiday);
        }

        [HttpPost("AddObjectHoliday")]
        public async Task<IActionResult> AddObjectHoliday([FromBody] ObjectHolidayDTO dto)
        {
            var exists = await _context.TblObjectHoliday
                .AnyAsync(x => x.holiday_id == dto.HolidayId && x.object_id == dto.ObjectId);

            if (exists)
                return BadRequest("Holiday already assigned to this object.");

            var objectHoliday = new tbl_ObjectHoliday
            {
                holiday_id = dto.HolidayId,
                object_id = dto.ObjectId
            };

            _context.TblObjectHoliday.Add(objectHoliday);
            await _context.SaveChangesAsync();

            return Ok(objectHoliday);
        }
    }

    public class ObjectHolidayDTO
    {
        public int ObjectId { get; set; }
        public int HolidayId { get; set; }
    }
}
