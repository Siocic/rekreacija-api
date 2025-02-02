namespace eRekreacija.Services.Interfaces
{
    public  interface ICRUDService<TModelDTO,TInsert,TUpdate>:IService<TModelDTO>
    {
        Task<TModelDTO> Insert(TInsert model);
        TModelDTO Update(int id,TUpdate model);
        void Delete(int id);    
    }
}
