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
        }
    }
}
