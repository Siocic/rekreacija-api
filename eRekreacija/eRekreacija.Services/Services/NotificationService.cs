using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;

namespace eRekreacija.Services.Services
{
    public  class NotificationService: BaseCRUDService<tbl_Notification,NotificationDTO,NotificationInsertRequest,object>, INotificationService
    {
        public NotificationService(RekreacijaContext rekreacijaContext,IMapper mapper) : base(rekreacijaContext, mapper){}
    }
}
