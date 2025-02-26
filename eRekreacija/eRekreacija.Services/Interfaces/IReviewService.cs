using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;

namespace eRekreacija.Services.Interfaces
{
    public interface IReviewService:ICRUDService<ReviewDTO, ReviewInsertRequest, object>
    {
        Task<List<ReviewDTO>> GetReviewOfObject(int object_id);
        Task<List<ReviewDTO>> GetReviewsForMyObjects(string userId);
    }
}