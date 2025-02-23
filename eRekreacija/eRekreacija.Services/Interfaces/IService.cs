namespace eRekreacija.Services.Interfaces
{
    public interface IService<TModelDTO>
    {
        Task<List<TModelDTO>> Get();
        public TModelDTO GetById(int id);
    }
}
