using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
   [ApiController]
    public class NotificationController : BaseCRUDController<NotificationDTO,NotificationInsertRequest,object>
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService):base(notificationService) {}
    }
}
