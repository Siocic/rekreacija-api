using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController<TModelDTO> : ControllerBase
    {
        private IService<TModelDTO> _service;
        public BaseController(IService<TModelDTO> service)
        {
            _service = service;
        }
        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            var reuslt= _service.Get();
            return Ok(reuslt);
        }
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) 
        {
            var result= _service.GetById(id);
            if (result == null)
            {
                return NotFound($"Entity with ID {id} not found");
            }
            return Ok(result);
        }
    }
}
