using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eRekreacija.Services.Services
{
    public class FavoritesService : BaseCRUDService<tbl_Favorites, FavoritesDTO, FavoritesInsertRequest, FavoritesUpdateRequest>, IFavoritesService
    {
        public FavoritesService(IdentityContext identityContext, IMapper mapper) : base(identityContext, mapper) { }

        public override async Task<FavoritesDTO> Insert(FavoritesInsertRequest model)
        {
            var checkIsAlreadyFavorite = await _identityContext.Set<tbl_Favorites>().FirstOrDefaultAsync(s => s.user_id == model.user_id && s.object_id == model.object_id);
            if (checkIsAlreadyFavorite != null)
            {
                _identityContext.Remove(checkIsAlreadyFavorite);
                await _identityContext.SaveChangesAsync();
                return null;
            }

            var entity = _mapper.Map<tbl_Favorites>(model);
            await _identityContext.Set<tbl_Favorites>().AddAsync(entity);
            await _identityContext.SaveChangesAsync();
            return _mapper.Map<FavoritesDTO>(entity);

        }
    }
}