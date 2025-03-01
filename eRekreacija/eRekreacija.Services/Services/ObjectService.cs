using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eRekreacija.Services.Services
{
    public class ObjectService : BaseCRUDService<tbl_Objects, ObjectsDTO, ObjectInsertRequest, ObjectUpdateRequest>, IObjectService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _host;

        public ObjectService(RekreacijaContext rekreacijaContext, IMapper mapper, UserManager<ApplicationUser> userManager, IHostingEnvironment host) : base(rekreacijaContext, mapper)
        {
            _host = host;
            _userManager = userManager;
        }

        public override async Task BeforeImageInsert(tbl_Objects entity, ObjectInsertRequest insert)
        {
            string cleanedName = string.IsNullOrWhiteSpace(insert.name)
                   ? "defaultobject"
                   : new string(insert.name.Where(char.IsLetterOrDigit).ToArray()).ToLower();

            string fileName = $"{cleanedName}.jpg";
            string filePath = Path.Combine(_host.WebRootPath, "images", fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, insert.ObjectImage);
            entity.ImagePath = $"/images/{fileName}";

            await base.BeforeImageInsert(entity, insert);
        }
        public override async Task BeforeImageUpdate(tbl_Objects entity, ObjectUpdateRequest update)
        {
            if(update.ObjectImage!=null)
            {
                string imagePath = Path.Combine(_host.WebRootPath, entity.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);

                string cleanedName = string.IsNullOrWhiteSpace(update.name)
               ? "defaultobject"
               : new string(update.name.Where(char.IsLetterOrDigit).ToArray()).ToLower();

                string fileName = $"{cleanedName}.jpg";
                string filePath = Path.Combine(_host.WebRootPath, "images", fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, update.ObjectImage);
                entity.ImagePath = $"/images/{fileName}";

                await base.BeforeImageUpdate(entity, update);
            }
            else
                await base.BeforeImageUpdate(entity, update);
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
                   await _rekreacijaContext.Set<tbl_ObjectSportCategory>().AddAsync(objectSportCategory);
                }

                await _rekreacijaContext.SaveChangesAsync();
            }
        }
        public override async Task BeforeUpdate(tbl_Objects entity, ObjectUpdateRequest update)
        {
            var findSportsId = _rekreacijaContext.Set<tbl_ObjectSportCategory>().Where(s => s.object_id == entity.id).ToList();
            if (findSportsId.Count() != 0)
            {
                _rekreacijaContext.RemoveRange(findSportsId);

                if (update.sportId != null && update.sportId.Any())
                {
                    foreach (var id in update.sportId)
                    {
                        var objectSportCategory = new tbl_ObjectSportCategory
                        {
                            object_id = entity.id,
                            sport_category_id = id
                        };
                       await _rekreacijaContext.Set<tbl_ObjectSportCategory>().AddAsync(objectSportCategory);
                    }

                   await _rekreacijaContext.SaveChangesAsync();
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
                        await _rekreacijaContext.Set<tbl_ObjectSportCategory>().AddAsync(objectSportCategory);
                    }

                    await _rekreacijaContext.SaveChangesAsync();
                }
            }
            await base.BeforeUpdate(entity, update);
        }
        public override async Task BeforeDelete(tbl_Objects db)
        {
            string imagePath = Path.Combine(_host.WebRootPath, db.ImagePath.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            await base.BeforeDelete(db);
        }
        public async Task<List<ObjectsDTO>> GetAllObjectsOfUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId);

            var objects = await _rekreacijaContext.TblObject.Where(s => s.user_id == userId).OrderByDescending(s=>s.created_date).Include(s => s.ObjectSportCategory).ToListAsync();

            var objectDTO = objects.Select(obj => new ObjectsDTO
            {
                id = obj.id,
                name = obj.name,
                address = obj.address,
                city = obj.city,
                description = obj.description,
                ImagePath=obj.ImagePath,
                price = obj.price,
                sportsId = obj.ObjectSportCategory.Select(s => s.sport_category_id).ToList(),
            }).ToList();
            return objectDTO;
        }

        public async Task<List<ObjectsDTO>> GetObjectByCategory(string userId, int categoryId, string? name = null)
        {
            var objectsIds = await _rekreacijaContext.Set<tbl_ObjectSportCategory>().Where(s => s.sport_category_id == categoryId).Select(s => s.object_id).ToListAsync();

            var userFavorites = await _rekreacijaContext.Set<tbl_Favorites>().Where(f => f.user_id == userId).Select(f => f.object_id).ToListAsync();

            var objectsDTO = await _rekreacijaContext.Set<tbl_Objects>()
                               .Where(s => objectsIds.Contains(s.id) && (string.IsNullOrEmpty(name) || s.name.Contains(name)))

                .Select(obj => new ObjectsDTO
                {
                    id = obj.id,
                    name = obj.name,
                    address = obj.address,
                    city = obj.city,
                    description = obj.description,
                    ImagePath=obj.ImagePath,
                    price = obj.price,
                    rating = obj.Reviews.Any() ? obj.Reviews.Average(r => r.rating) : 0,
                    isFavorites = userFavorites.Contains(obj.id)
                }).ToListAsync();

            return objectsDTO;
        }

        public async Task<List<ObjectsDTO>> GetFavoritesObject(string userId)
        {
            var objectId = await _rekreacijaContext.Set<tbl_Favorites>().Where(s => s.user_id == userId).Select(s => s.object_id).ToListAsync();

            var objects = await _rekreacijaContext.Set<tbl_Objects>().Where(s => objectId.Contains(s.id)).Include(s => s.Reviews).ToListAsync();

            var objectsDTO = await _rekreacijaContext.Set<tbl_Objects>()
                .Where(s => objectId.Contains(s.id))
            .Include(s => s.ObjectSportCategory)
            .Select(obj => new ObjectsDTO
            {
                id = obj.id,
                name = obj.name,
                address = obj.address,
                city = obj.city,
                description = obj.description,
                ImagePath=obj.ImagePath,
                price = obj.price,
                rating = obj.Reviews.Any() ? obj.Reviews.Average(r => r.rating) : 0,
                sportsId = obj.ObjectSportCategory.Select(s => s.sport_category_id).ToList(),
            }).ToListAsync();

            return objectsDTO;
        }

        public override async Task<List<ObjectsDTO>> Get()
        {
            var objectsDTO = await _rekreacijaContext.Set<tbl_Objects>()
               .Select(obj => new ObjectsDTO
               {
                   id = obj.id,
                   name = obj.name,
                   address = obj.address,
                   city = obj.city,
                   description = obj.description,
                   ImagePath = obj.ImagePath,
                   price = obj.price,
                   rating = obj.Reviews.Any() ? obj.Reviews.Average(r => r.rating) : 0,
               }).ToListAsync();

            return objectsDTO;
        }
    }
}