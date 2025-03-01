namespace eRekreacija.Services.Interfaces
{
    public  interface ICRUDService<TModelDTO,TInsert,TUpdate>:IService<TModelDTO>
    {
        Task<TModelDTO> Insert(TInsert model);
        Task<TModelDTO> Update(int id,TUpdate model);
        Task<bool> Delete(int id);    
    }
}
