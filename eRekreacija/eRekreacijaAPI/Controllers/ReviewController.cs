using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    public class ReviewController : BaseCRUDController<ReviewDTO,ReviewInsertRequest,object>
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService) :base(reviewService) {
            _reviewService = reviewService;
        }
    }
}