using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Services.IAdminService;

namespace VOD.API.Controllers
{
    [Route("api/courses/{courseId}/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        #region Properties and Variables
        private readonly IAdminService _db;
        private readonly LinkGenerator _linkGenerator;
        #endregion
        #region Constructor
        public ModulesController(IAdminService db,
        LinkGenerator linkGenerator)
        {
            _db = db;
            _linkGenerator = linkGenerator;
        }
        #endregion
        #region Actions
        [HttpGet()]
        public async Task<ActionResult<List<ModuleDTO>>> Get(int courseId,
        bool include = false)
        {
            try
            {
                var dtos = courseId == 0 ?
                    await _db.GetAsync<Module, ModuleDTO>(include) :
                    await _db.GetAsync<Module, ModuleDTO>(m => m.CourseId.Equals(courseId), include);
                if (!include)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Downloads = null;
                        dto.Videos = null;
                    }
                }
                return dtos;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Database Failure");
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ModuleDTO>> Get(int courseId, int id,
            bool include = false)
        {
            try
            {
                var dto = courseId==0? 
                    await _db.SingleAsync<Module, ModuleDTO>(s => s.Id.Equals(id), include):
                    await _db.SingleAsync<Module, ModuleDTO>(m=>m.Id.Equals(id) && 
                        m.CourseId.Equals(courseId), include);
                if (dto == null) return NotFound();
                if (!include)
                {
                    dto.Downloads = null;
                    dto.Videos = null;
                }  
                return dto;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Database Failure");
            }
        }
        [HttpPost]
        public async Task<ActionResult<ModuleDTO>> Post(int courseId, ModuleDTO model)
        {
            try
            {
                if (courseId.Equals(0)) courseId = model.CourseId;
                if (model == null) return BadRequest("No entity provided");
                if (!model.CourseId.Equals(courseId)) return BadRequest("Differing ids");
                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(model.CourseId));
                if (!exists) return NotFound("Could not find related entity");
                var id = await _db.CreateAsync<ModuleDTO, Module>(model);
                if (id < 1) return BadRequest("Unable to add the entity");
                var dto = await _db.SingleAsync<Module, ModuleDTO>(s =>
                        s.CourseId.Equals(courseId) && s.Id.Equals(id));
                if (dto == null) return BadRequest("Unable to add the entity");
                var uri = _linkGenerator.GetPathByAction("Get", "Modules",
                    new { courseId, id });
                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Failed to add the entity");
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int courseId, int id, ModuleDTO model)
        {
            try
            {
                if (model == null) return BadRequest("Missing entity");
                if (!id.Equals(model.Id)) return BadRequest(
                "Differing ids");
                var exists = await _db.AnyAsync<Course>(a =>
                     a.Id.Equals(model.CourseId));
                if (!exists) return NotFound("Could not find related entity");
                exists = await _db.AnyAsync<Module>(a => a.Id.Equals(id));
                if (!exists) return NotFound("Could not find entity");
                if (await _db.UpdateAsync<ModuleDTO, Module>(model))
                    return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Failed to update the entity");
            }
            return BadRequest("Unable to update the entity");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int courseId, int id)
        {
            try
            {
                var exists = await _db.AnyAsync<Module>(a => 
                    a.CourseId.Equals(courseId) && a.Id.Equals(id));
                if (!exists) return BadRequest("Could not find entity");
                if (await _db.DeleteAsync<Module>(d => d.Id.Equals(id) && d.CourseId.Equals(courseId)) )
                    return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Failed to delete the entity");
            }
            return BadRequest("After catch Failed to delete the entity");
        }
        #endregion
    }
}