using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    public class AppointmentController : BaseCRUDController<AppointmentDTO,AppointmentInsertRequest,object>
    {
        public AppointmentController(IAppointmentService appointmentService):base(appointmentService){}
    }
}