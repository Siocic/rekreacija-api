using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;

namespace eRekreacija.Services.Interfaces
{
    public interface IFavoritesService:ICRUDService<FavoritesDTO,FavoritesInsertRequest,FavoritesUpdateRequest>
    {
    }
}
