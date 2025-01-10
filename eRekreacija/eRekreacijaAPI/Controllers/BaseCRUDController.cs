using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseCRUDController<TModelDTO, TInsert, TUpdate> : BaseController<TModelDTO>
    {
        protected new readonly ICRUDService<TModelDTO, TInsert, TUpdate> _crudService;

        public BaseCRUDController(ICRUDService<TModelDTO, TInsert, TUpdate> crudService) : base(crudService) { }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Insert")]
        public IActionResult Insert([FromBody] TInsert model)
        {
            var result = _crudService.Insert(model);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] TUpdate model)
        {
            var result = _crudService.Update(id, model);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _crudService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
