using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;

namespace eRekreacija.Services.Interfaces
{
    public  interface INotificationService:ICRUDService<NotificationDTO, NotificationInsertRequest,object>
    {
    }
}
