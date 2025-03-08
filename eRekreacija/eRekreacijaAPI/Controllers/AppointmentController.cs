using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    public class AppointmentController : BaseCRUDController<AppointmentDTO, AppointmentInsertRequest, object>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentController> _logger;
        public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger) : base(appointmentService)
        {
            _appointmentService = appointmentService;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetAppointments")]
        public async Task<IActionResult> GetAppointments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _appointmentService.GetAppointmentOfObject(userId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("ApproveAppointment")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            if (id == 0)
                return BadRequest("Invalid id");

            try
            {
                var result = await _appointmentService.ApproveAppointment(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, ex.InnerException?.Message, ex.Message);
                return BadRequest();
            }
        }
    }
}