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

        public virtual TModelDTO Insert(TInsert model)
        {
            var entity = _mapper.Map<TModel>(model);
            _rekreacijaContext.Set<TModel>().Add(entity);
            _rekreacijaContext.SaveChanges();

            return _mapper.Map<TModelDTO>(entity);
        }

        public virtual TModelDTO Update(int id, TUpdate model)
        {
            var entity = _rekreacijaContext.Set<TModel>().Find(id);
            if (entity == null)
                throw new Exception($"Enttity with ID:{id} not found");

            _mapper.Map(model,entity);
            _rekreacijaContext.SaveChanges();

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
