using AutoMapper;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Interfaces;

namespace eRekreacija.Services.Services
{
    public abstract class BaseCRUDService<TModel, TModelDTO, TInsert, TUpdate> : BaseService<TModel, TModelDTO>, ICRUDService<TModelDTO, TInsert, TUpdate>
        where TModel : class
        where TModelDTO : class
    {
        public BaseCRUDService(IdentityContext identityContext, IMapper mapper) : base(identityContext, mapper) { }

        public virtual async Task BeforeInsert(TModel db, TInsert insert) { }
        public virtual async Task BeforeUpdate(TModel db, TUpdate insert) { }
        public virtual async Task BeforeImageInsert(TModel db, TInsert insert) { }
        public virtual async Task BeforeDelete(TModel db) { }
        public virtual async Task BeforeImageUpdate(TModel db, TUpdate update) { }

        public virtual async Task<TModelDTO> Insert(TInsert model)
        {
            var entity = _mapper.Map<TModel>(model);
            await BeforeImageInsert(entity, model);
            await _identityContext.Set<TModel>().AddAsync(entity);

            await _identityContext.SaveChangesAsync();
            await BeforeInsert(entity, model);

            return _mapper.Map<TModelDTO>(entity);
        }

        public virtual async Task<TModelDTO> Update(int id, TUpdate model)
        {
            try
            {
                var entity = await _identityContext.Set<TModel>().FindAsync(id);
                if (entity == null)
                    throw new Exception($"Enttity with ID:{id} not found");

                _mapper.Map(model, entity);
                await BeforeImageUpdate(entity, model);

                await _identityContext.SaveChangesAsync();
                await BeforeUpdate(entity, model);

                return _mapper.Map<TModelDTO>(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public virtual async Task<bool> Delete(int id)
        {
            var entity = await _identityContext.Set<TModel>().FindAsync(id);
            if (entity == null)
                throw new Exception($"Enttity with ID:{id} not found");

            await BeforeDelete(entity);
            _identityContext.Set<TModel>().Remove(entity);
            await _identityContext.SaveChangesAsync();
            return true;
        }

    }
}
