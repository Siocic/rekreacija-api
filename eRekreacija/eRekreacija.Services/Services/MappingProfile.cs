using AutoMapper;
using eRekreacija.Services.Database;
using eRekreacija.Models.Models;

namespace eRekreacija.Services.Services
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, RegisterRequest>();
            CreateMap<RegisterRequest, ApplicationUser>();
        }
    }
}
