using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace eRekreacija.Services.Services
{
    public class ObjectService : BaseCRUDService<tbl_Objects, ObjectsDTO, ObjectInsertRequest, ObjectUpdateRequest>, IObjectService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ObjectService(RekreacijaContext rekreacijaContext, IMapper mapper, UserManager<ApplicationUser> userManager) : base(rekreacijaContext, mapper)
        {
            _userManager = userManager;
        }

        public override Task BeforeImageInsert(tbl_Objects entity, ObjectInsertRequest insert)
        {
            if (insert.ObjectImage == null)
                entity.ObjectImage = null;

            return base.BeforeImageInsert(entity, insert);
        }
        public override async Task BeforeInsert(tbl_Objects entity, ObjectInsertRequest insert)
        {
            if (insert.sportId != null && insert.sportId.Any())
            {
                foreach (var id in insert.sportId)
                {
                    var objectSportCategory = new tbl_ObjectSportCategory
                    {
                        object_id = entity.id,
                        sport_category_id = id
                    };
                    _rekreacijaContext.Set<tbl_ObjectSportCategory>().Add(objectSportCategory);
                }

                await _rekreacijaContext.SaveChangesAsync();
            }
        }
        public override Task BeforeUpdate(tbl_Objects entity, ObjectUpdateRequest update)
        {
            var findSportsId = _rekreacijaContext.Set<tbl_ObjectSportCategory>().Where(s => s.object_id == entity.id).ToList();
            if (findSportsId.Count() != 0)
            {
                _rekreacijaContext.Remove(findSportsId);

                if (update.sportId != null && update.sportId.Any())
                {
                    foreach (var id in update.sportId)
                    {
                        var objectSportCategory = new tbl_ObjectSportCategory
                        {
                            object_id = entity.id,
                            sport_category_id = id
                        };
                        _rekreacijaContext.Set<tbl_ObjectSportCategory>().Add(objectSportCategory);
                    }

                    _rekreacijaContext.SaveChangesAsync();
                }
            }
            else
            {
                if (update.sportId != null && update.sportId.Any())
                {
                    foreach (var id in update.sportId)
                    {
                        var objectSportCategory = new tbl_ObjectSportCategory
                        {
                            object_id = entity.id,
                            sport_category_id = id
                        };
                        _rekreacijaContext.Set<tbl_ObjectSportCategory>().Add(objectSportCategory);
                    }

                    _rekreacijaContext.SaveChangesAsync();
                }
            }
            return base.BeforeUpdate(entity, update);
        }
        public async Task<List<ObjectsDTO>> GetAllObjectsOfUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId);

            var objects = await _rekreacijaContext.TblObject.Where(s => s.user_id == userId).Include(s => s.ObjectSportCategory).ToListAsync();

            var objectDTO = objects.Select(obj => new ObjectsDTO
            {
                id = obj.id,
                name = obj.name,
                address = obj.address,
                city = obj.city,
                description = obj.description,
                ObjectImage = obj.ObjectImage,
                price = obj.price,
                sportsId = obj.ObjectSportCategory.Select(s => s.sport_category_id).ToList(),
            }).ToList();
            return objectDTO;
        }

        public async Task<List<ObjectsDTO>>GetForUser(string userId)
        {
            var objects = await _rekreacijaContext.Set<tbl_Objects>().Include(s => s.Reviews).Include(s => s.ObjectSportCategory).ToListAsync();

            var userFavorites = await _rekreacijaContext.Set<tbl_Favorites>().Where(f => f.user_id == userId).Select(f => f.object_id).ToListAsync();

            var objectsDTO = await _rekreacijaContext.Set<tbl_Objects>()
                .Include(s => s.ObjectSportCategory)
                .Select(obj => new ObjectsDTO
                {
                    id = obj.id,
                    name = obj.name,
                    address = obj.address,
                    city = obj.city,
                    description = obj.description,
                    ObjectImage = obj.ObjectImage,
                    price = obj.price,
                    rating = obj.Reviews.Any() ? obj.Reviews.Average(r => r.rating) : 0,
                    sportsId = obj.ObjectSportCategory.Select(s => s.sport_category_id).ToList(),
                    isFavorites = userFavorites.Contains(obj.id)
                }).ToListAsync();

            return objectsDTO;
        }

        public async Task<List<ObjectsDTO>> GetFavoritesObject(string userId)
        {
            var objectId = await _rekreacijaContext.Set<tbl_Favorites>().Where(s => s.user_id == userId).Select(s=>s.object_id).ToListAsync();

            var objects = await _rekreacijaContext.Set<tbl_Objects>().Where(s => objectId.Contains(s.id)).Include(s=>s.Reviews).ToListAsync();

            var objectsDTO = await _rekreacijaContext.Set<tbl_Objects>()
                .Where(s=>objectId.Contains(s.id))
            .Include(s => s.ObjectSportCategory)
            .Select(obj => new ObjectsDTO
            {
                id = obj.id,
                name = obj.name,
                address = obj.address,
                city = obj.city,
                description = obj.description,
                ObjectImage = obj.ObjectImage,
                price = obj.price,
                rating = obj.Reviews.Any() ? obj.Reviews.Average(r => r.rating) : 0,
                sportsId = obj.ObjectSportCategory.Select(s => s.sport_category_id).ToList(),
            }).ToListAsync();

            return objectsDTO;
        }
    }
}