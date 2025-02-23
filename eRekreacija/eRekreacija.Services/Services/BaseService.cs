using AutoMapper;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eRekreacija.Services.Services
{
    public abstract class BaseService<TModel,TModelDTO>: IService<TModelDTO> where TModel : class where TModelDTO : class
    {
        protected readonly RekreacijaContext _rekreacijaContext;
        protected readonly IMapper _mapper;

        public BaseService(RekreacijaContext rekreacijaContext,IMapper mapper)
        {
            _mapper = mapper;
            _rekreacijaContext = rekreacijaContext;
        }

        public virtual async Task<List<TModelDTO>> Get()
        {
           var entities= await _rekreacijaContext.Set<TModel>().ToListAsync();
            return _mapper.Map<List<TModelDTO>>(entities);    
        }

        public TModelDTO GetById(int id)
        {
            var entity = _rekreacijaContext.Set<TModel>().Find(id);
            if(entity!=null)
            {
                return _mapper.Map<TModelDTO>(entity);
            }
            else
            {
                return null;
            }
        }
    }
}
