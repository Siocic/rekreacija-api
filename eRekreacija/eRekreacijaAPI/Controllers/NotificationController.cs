using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
using eRekreacija.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eRekreacijaAPI.Controllers
{
   [ApiController]
    public class NotificationController : BaseCRUDController<NotificationDTO,NotificationInsertRequest,object>
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService):base(notificationService) {
         _notificationService = notificationService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getNotificationsOfUser")]
        public async Task<IActionResult> GetnotificationsOfUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _notificationService.GetAllNotificatiosOfUser(userId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
