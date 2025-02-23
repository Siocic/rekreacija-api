using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;

namespace eRekreacija.Services.Services
{
    public class FavoritesService:BaseCRUDService<tbl_Favorites,FavoritesDTO,FavoritesInsertRequest,FavoritesUpdateRequest>,IFavoritesService
    {
        public FavoritesService(RekreacijaContext rekreacijaContext,IMapper mapper):base(rekreacijaContext,mapper){}
    }
}