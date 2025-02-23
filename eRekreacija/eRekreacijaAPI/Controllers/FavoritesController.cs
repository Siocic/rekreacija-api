using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    public class FavoritesController : BaseCRUDController<FavoritesDTO, FavoritesInsertRequest, FavoritesUpdateRequest>
    {
        public FavoritesController(IFavoritesService favoritesService) : base(favoritesService) { }
    }
}