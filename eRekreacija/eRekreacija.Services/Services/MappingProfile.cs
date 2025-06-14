using AutoMapper;
using eRekreacija.Services.Database;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Models.DTOs;
using eRekreacijaAPI.DTOs;

namespace eRekreacija.Services.Services
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, RegisterRequest>();
            CreateMap<RegisterRequest, ApplicationUser>();
            CreateMap<tbl_SportCategory, SportCategoryDTO>().ReverseMap();

            #region MAPPING-PROFILES-FOR-OBJECTS
            CreateMap<tbl_Objects, ObjectsDTO>().ReverseMap();
            CreateMap<ObjectInsertRequest, tbl_Objects>();
            CreateMap<ObjectUpdateRequest, tbl_Objects>();
            #endregion

            #region MAPPING-PROFILES-FOR-NOTIFICATIONS
            CreateMap<tbl_Notification, NotificationDTO>().ReverseMap();
            CreateMap<NotificationInsertRequest, tbl_Notification>();
            #endregion

            #region MAPPING-PROFILES-FOR-REVIEWS
            CreateMap<tbl_Review, ReviewDTO>().ReverseMap();
            CreateMap<ReviewInsertRequest, tbl_Review>();
            #endregion

            #region MAPPING-PROFILES-FOR-FAVORITES
            CreateMap<tbl_Favorites, FavoritesDTO>().ReverseMap();
            CreateMap<FavoritesInsertRequest, tbl_Favorites>();
            CreateMap<FavoritesUpdateRequest, tbl_Favorites>();
            #endregion

            #region MAPPING-PROFILES-FOR-APPOINTMENTS
            CreateMap<tbl_Appointment, AppointmentDTO>().ReverseMap();
            CreateMap<AppointmentInsertRequest, tbl_Appointment>();
            #endregion

            #region MAPPING-PROFILES-FOR-CHAT
            CreateMap<ChatMessageDTO, tbl_ChatMessage>();
            CreateMap<tbl_ChatMessage, ChatMessageDTO>();
            #endregion

            #region MAPPING-PROFILES-FOR-HOLIDAY
            CreateMap<HolidayDTO, tbl_Holiday>();
            CreateMap<tbl_Holiday, HolidayDTO>();
            CreateMap<ObjectHolidayDTO, tbl_ObjectHoliday>();
            CreateMap<tbl_ObjectHoliday, ObjectHolidayDTO>();
            #endregion
        }
    }
}