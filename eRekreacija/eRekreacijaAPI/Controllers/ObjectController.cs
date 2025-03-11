using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    public class ObjectController : BaseCRUDController<ObjectsDTO,ObjectInsertRequest,ObjectUpdateRequest>
    {
        private readonly IObjectService _objectService;
        public ObjectController(IObjectService objectService):base(objectService){
            _objectService = objectService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getObjectsOfUser")]
        public async Task<IActionResult> GetObjectsOfUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _objectService.GetAllObjectsOfUser(userId);
            if(result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getObjects/{categoryId}")]
        public async Task<IActionResult> getObjects(int categoryId, [FromQuery]string? name=null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _objectService.GetObjectByCategory(userId,categoryId,name);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getFavoritesObjectOfUser")]
        public async Task<IActionResult> GetFavoritesObject()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _objectService.GetFavoritesObject(userId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getRecommended")]
        public async Task<IActionResult>GetRecommended()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();
            Guid userGuid = Guid.Parse(userId);

            try
            {
                var result = _objectService.Recomended(userId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}