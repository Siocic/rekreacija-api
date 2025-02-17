using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;

namespace eRekreacija.Services.Interfaces
{
    public interface IReviewService:ICRUDService<ReviewDTO, ReviewInsertRequest, object>
    {
    }
}