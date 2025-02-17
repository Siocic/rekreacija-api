using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;

namespace eRekreacija.Services.Services
{
    public class ReviewService:BaseCRUDService<tbl_Review, ReviewDTO, ReviewInsertRequest, object>,IReviewService
    {
        public ReviewService(RekreacijaContext rekreacijaContext,IMapper mapper):base(rekreacijaContext,mapper) {}
    }
}
