using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;

namespace eRekreacija.Services.Services
{
    public class ObjectService:BaseCRUDService<tbl_Objects,ObjectsDTO,ObjectInsertRequest,ObjectUpdateRequest>,IObjectService
    {
        public ObjectService(RekreacijaContext rekreacijaContext,IMapper mapper):base(rekreacijaContext,mapper){}

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
    }
}
