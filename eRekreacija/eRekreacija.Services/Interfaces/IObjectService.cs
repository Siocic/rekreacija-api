using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;

namespace eRekreacija.Services.Interfaces
{
    public interface IObjectService:ICRUDService<ObjectsDTO,ObjectInsertRequest,ObjectUpdateRequest>
    {
        Task<List<ObjectsDTO>> GetAllObjectsOfUser(string userId);
    }
}
