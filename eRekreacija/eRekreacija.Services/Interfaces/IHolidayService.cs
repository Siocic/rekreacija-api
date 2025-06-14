using eRekreacija.Services.Database.Entities;
using eRekreacijaAPI.DTOs;

namespace eRekreacijaAPI.Services.Interfaces
{
    public interface IHolidayService
    {
        Task<HolidayDTO> AddHolidayAsync(HolidayDTO dto);
        Task<tbl_ObjectHoliday?> AddObjectHolidayAsync(ObjectHolidayDTO dto);
        Task<List<HolidayDTO>> GetAllHolidaysAsync();
        Task<List<HolidayDTO>> GetHolidaysByObjectIdAsync(int objectId);

    }
}
