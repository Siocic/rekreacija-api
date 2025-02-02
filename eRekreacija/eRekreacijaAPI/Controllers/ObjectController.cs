using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    public class ObjectController : BaseCRUDController<ObjectsDTO,ObjectInsertRequest,ObjectUpdateRequest>
    {
        public ObjectController(IObjectService objectService):base(objectService){}     
       
    }
}
