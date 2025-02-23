using AutoMapper;
using eRekreacija.Services.Database;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Models.DTOs;

namespace eRekreacija.Services.Services
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, RegisterRequest>();
            CreateMap<RegisterRequest, ApplicationUser>();
            CreateMap<tbl_SportCategory,SportCategoryDTO>().ReverseMap();

            #region MAPPING-PROFILES-FOR-OBJECTS
            CreateMap<tbl_Objects, ObjectsDTO>().ReverseMap();
            CreateMap<ObjectInsertRequest, tbl_Objects>();
            CreateMap<ObjectUpdateRequest, tbl_Objects>();
            #endregion

            #region MAPPING-PROFILES-FOR-NOTIFICATIONS
            CreateMap<tbl_Notification,NotificationDTO>().ReverseMap();
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
        }
    }
}