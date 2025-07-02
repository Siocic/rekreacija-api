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

        public NotificationService(IdentityContext identityContext,IMapper mapper, UserManager<ApplicationUser> userManager) : base(identityContext, mapper){
            _userManager = userManager;
        }

        public override async Task<List<NotificationDTO>> Get()
        {
            var notifications = await _identityContext.Set<tbl_Notification>().OrderByDescending(s => s.created_date).Select(s => new NotificationDTO
            {
                created_date=s.created_date,
                name=s.name,
                description=s.description,
                user_id=s.user_id,
            }).ToListAsync();

            if (!notifications.Any())
                return new List<NotificationDTO>();

            var userIds = notifications.Select(s => s.user_id).Distinct().ToList();

            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id))
                .Select(u => new ApplicationUserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                }).ToListAsync();

            var userDict = users.ToDictionary(u => u.Id, u => u);
            foreach(var notification in notifications)
            {
                if (userDict.TryGetValue(notification.user_id, out var user))
                    notification.user = user;
            }

            return notifications;
        }

        public async Task<List<NotificationDTO>> GetAllNotificatiosOfUser(string userId)
        {
            //var user = _userManager.FindByIdAsync(userId);

            var notification = await _identityContext.TblNotification.Where(s => s.user_id == userId).ToListAsync();

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
