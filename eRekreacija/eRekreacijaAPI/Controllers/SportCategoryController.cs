using eRekreacija.Models.DTOs;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SportCategoryController : BaseController<SportCategoryDTO>
    {
        public SportCategoryController(ISportCategoryService sportCategoryService) : base(sportCategoryService){}
    }
}
