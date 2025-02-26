using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    public class ReviewController : BaseCRUDController<ReviewDTO,ReviewInsertRequest,object>
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService) :base(reviewService) {
            _reviewService = reviewService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getReviewForObject/{objectId}")]
        public async Task<IActionResult> GetReviewForObject(int objectId)
        {
            if (objectId == 0)
                return BadRequest();

            var result = await _reviewService.GetReviewOfObject(objectId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getReviewsForMyObjects")]
        public async Task<IActionResult> GetReviewsForMyObjects()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _reviewService.GetReviewsForMyObjects(userId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}