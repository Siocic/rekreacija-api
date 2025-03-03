using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;

namespace eRekreacija.Services.Interfaces
{
    public interface IAppointmentService:ICRUDService<AppointmentDTO, AppointmentInsertRequest,object>
    {
    }
}