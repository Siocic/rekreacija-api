namespace eRekreacija.Services.Interfaces
{
    public interface IService<TModelDTO>
    {
        public List<TModelDTO> Get();
        public TModelDTO GetById(int id);
    }
}
