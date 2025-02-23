using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eRekreacija.Services.Services
{
    public class ReviewService : BaseCRUDService<tbl_Review, ReviewDTO, ReviewInsertRequest, object>, IReviewService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewService(RekreacijaContext rekreacijaContext, IMapper mapper, UserManager<ApplicationUser> userManager) : base(rekreacijaContext, mapper) {
            _userManager = userManager;
        }

        public async Task<List<ReviewDTO>> GetReviewOfObject(int object_id)
        {
            var reviews = await _rekreacijaContext.Set<tbl_Review>()
                .Where(s => s.object_id == object_id)
                .OrderByDescending(s => s.created_date)
                .Take(6)
                .Select(s => new ReviewDTO
                {
                    id = s.id,
                    comment = s.comment,
                    rating = s.rating,
                    created_date = s.created_date,
                    user_id = s.user_id,
                    object_id = s.object_id
                }).ToListAsync();

            if (!reviews.Any())
                return new List<ReviewDTO>();

            var userIds = reviews.Select(r => r.user_id).Distinct().ToList();

            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id))
                .Select(u => new ApplicationUserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfilePicture = u.ProfilePicutre != null ? Convert.ToBase64String(u.ProfilePicutre) : null
                }).ToListAsync();

            var userDict = users.ToDictionary(u => u.Id, u => u);

            foreach(var review in reviews)
            {
                if (userDict.TryGetValue(review.user_id, out var user))
                    review.user = user;
            }

            return reviews;
        }
    }
}
