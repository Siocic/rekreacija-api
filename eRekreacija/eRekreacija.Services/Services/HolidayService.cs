using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacijaAPI.DTOs;
using eRekreacijaAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eRekreacijaAPI.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IdentityContext _context;
        public HolidayService(IdentityContext context)
        {
            _context = context;
        }

        public async Task<HolidayDTO> AddHolidayAsync(HolidayDTO holiday)
        {
            var newHoliday = new tbl_Holiday
            {
                name = holiday.name,
                start_date = holiday.start_date,
                end_date = holiday.end_date
            };

            _context.TblHoliday.Add(newHoliday);
            await _context.SaveChangesAsync();

            holiday.id = newHoliday.id;

            return new HolidayDTO
            {
                id = newHoliday.id,
                name = newHoliday.name,
                start_date = newHoliday.start_date,
                end_date = newHoliday.end_date
            };
        }

        public async Task<tbl_ObjectHoliday?> AddObjectHolidayAsync(ObjectHolidayDTO dto)
        {
            var exists = await _context.TblObjectHoliday
                .AnyAsync(x => x.holiday_id == dto.HolidayId && x.object_id == dto.ObjectId);

            if (exists)
                return null;

            var objectHoliday = new tbl_ObjectHoliday
            {
                holiday_id = dto.HolidayId,
                object_id = dto.ObjectId
            };

            _context.TblObjectHoliday.Add(objectHoliday);
            await _context.SaveChangesAsync();

            return objectHoliday;
        }

        public async Task<List<HolidayDTO>> GetAllHolidaysAsync()
        {
            var holidays = await _context.TblHoliday
                .OrderBy(h => h.start_date)
                .ToListAsync();

            return holidays.Select(h => new HolidayDTO
            {
                id = h.id,
                name = h.name,
                start_date = h.start_date,
                end_date = h.end_date
            }).ToList();
        }

        public async Task<List<HolidayDTO>> GetHolidaysByObjectIdAsync(int objectId)
        {
            var holidays = await _context.TblObjectHoliday
                .Where(oh => oh.object_id == objectId)
                .Include(oh => oh.TblHoliday)
                .Select(oh => new HolidayDTO
                {
                    id = oh.TblHoliday.id,
                    name = oh.TblHoliday.name,
                    start_date = oh.TblHoliday.start_date,
                    end_date = oh.TblHoliday.end_date
                })
                .OrderBy(h => h.start_date)
                .ToListAsync();

            return holidays;
        }

    }
}
