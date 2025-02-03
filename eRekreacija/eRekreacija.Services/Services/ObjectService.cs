using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eRekreacija.Services.Services
{
    public class ObjectService:BaseCRUDService<tbl_Objects,ObjectsDTO,ObjectInsertRequest,ObjectUpdateRequest>,IObjectService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ObjectService(RekreacijaContext rekreacijaContext,IMapper mapper,UserManager<ApplicationUser> userManager):base(rekreacijaContext,mapper){
            _userManager = userManager;
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

        public async Task<List<ObjectsDTO>> GetAllObjectsOfUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId);

            var objects = await _rekreacijaContext.TblObject.Where(s => s.user_id == userId).Include(s=>s.ObjectSportCategory).ToListAsync();

            var objectDTO = objects.Select(obj => new ObjectsDTO
            {
                id = obj.id,
                name = obj.name,
                address = obj.address,
                city = obj.city,
                description = obj.description,
                ObjectImage = obj.ObjectImage,
                price = obj.price,
                sportsId=obj.ObjectSportCategory.Select(s=>s.sport_category_id).ToList(),
            }).ToList();
            return objectDTO;
        }
    }
}