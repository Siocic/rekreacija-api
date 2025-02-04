using AutoMapper;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Interfaces;

namespace eRekreacija.Services.Services
{
    public abstract class BaseCRUDService<TModel, TModelDTO, TInsert, TUpdate> : BaseService<TModel, TModelDTO>, ICRUDService<TModelDTO, TInsert, TUpdate>
        where TModel : class
        where TModelDTO : class
    {
        public BaseCRUDService(RekreacijaContext rekreacijaContext, IMapper mapper) : base(rekreacijaContext, mapper) { }

        public virtual async Task BeforeInsert(TModel db, TInsert insert)
        {

        }
        public virtual async Task BeforeUpdate(TModel db, TUpdate insert)
        {

        }

        public virtual async Task<TModelDTO> Insert(TInsert model)
        {
            var entity = _mapper.Map<TModel>(model);
            _rekreacijaContext.Set<TModel>().Add(entity);

            _rekreacijaContext.SaveChanges();
            await BeforeInsert(entity, model);

            return _mapper.Map<TModelDTO>(entity);
        }

        public virtual async Task<TModelDTO> Update(int id, TUpdate model)
        {
            var entity = _rekreacijaContext.Set<TModel>().Find(id);
            if (entity == null)
                throw new Exception($"Enttity with ID:{id} not found");

            _mapper.Map(model,entity);
            _rekreacijaContext.SaveChanges();
            await BeforeUpdate(entity, model);

            return _mapper.Map<TModelDTO>(entity);
        }
        public virtual void Delete(int id)
        {
            var entity = _rekreacijaContext.Set<TModel>().Find(id);
            if (entity == null)
                throw new Exception($"Enttity with ID:{id} not found");
            
            _rekreacijaContext.Set<TModel>().Remove(entity);
            _rekreacijaContext.SaveChanges();
        }
    }
}
