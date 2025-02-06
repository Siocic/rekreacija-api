using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eRekreacija.Services.Services
{
    public  class NotificationService: BaseCRUDService<tbl_Notification,NotificationDTO,NotificationInsertRequest,object>, INotificationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationService(RekreacijaContext rekreacijaContext,IMapper mapper, UserManager<ApplicationUser> userManager) : base(rekreacijaContext, mapper){
            _userManager = userManager;
        }

        public async Task<List<NotificationDTO>> GetAllNotificatiosOfUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId);

            var notification = await _rekreacijaContext.TblNotification.Where(s => s.user_id == userId).ToListAsync();

            var notificationDTO = notification.Select(s=>new NotificationDTO
            {
                id = s.id,
                name = s.name,
                description = s.description,
                created_date = s.created_date,
            }).OrderByDescending(s => s.created_date).ToList();    

            return notificationDTO;
        }
    }
}
