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

        public BaseCRUDController(ICRUDService<TModelDTO, TInsert, TUpdate> crudService) : base(crudService) {
            _crudService = crudService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] TInsert model)
        {
            try
            {
                var result = await _crudService.Insert(model);
                return Ok(new { Message = "You successfully add." });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TUpdate model)
        {
            try
            {
                var result = await _crudService.Update(id, model);
                return Ok(new { Message = "Your edit was successfully." });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }          
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _crudService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound(ex.Message);
            }
        }       
    }
}
