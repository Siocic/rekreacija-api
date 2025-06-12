using eRekreacija.Services.Database.Entities;
using eRekreacijaAPI.DTOs;

namespace eRekreacijaAPI.Services.Interfaces
{
    public interface IHolidayService
    {
        Task<tbl_Holiday> AddHolidayAsync(HolidayDTO dto);
        Task<tbl_ObjectHoliday?> AddObjectHolidayAsync(ObjectHolidayDTO dto);
    }
}
