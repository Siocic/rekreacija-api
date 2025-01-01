using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;

namespace eRekreacija.Services.Services
{
    public class SportCategoryService : BaseService<tbl_SportCategory, SportCategoryDTO>, ISportCategoryService
    {
        public SportCategoryService(RekreacijaContext rekreacijaContext,IMapper mapper):base(rekreacijaContext,mapper){}
    }
}
