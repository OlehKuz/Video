using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Common.Services.IAdminService;

namespace VOD.API.Controllers
{
    [Route("api/courses/{courseId}/modules/{moduleId}/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        #region Properties and Variables
        private readonly IAdminService _db;
        private readonly LinkGenerator _linkGenerator;
        #endregion
        #region Constructor
        public VideosController(IAdminService db, LinkGenerator linkGenerator)
        {
            _db = db;
            _linkGenerator = linkGenerator;
        }
        #endregion
        #region Actions
        [HttpGet()]
        public async Task<ActionResult<List<VideoDTO>>> Get(int courseId,int moduleId, bool include = false)
        {
            try
            {
                var dtos = courseId == 0 ?
                    await _db.GetAsync<Video, VideoDTO>(include) :
                    await _db.GetAsync<Video, VideoDTO>(m => m.CourseId.Equals(courseId) &&
                        m.ModuleId.Equals(moduleId), include);
                return dtos;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Database Failure");
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<VideoDTO>> Get(int courseId, int moduleId, int id,
            bool include = false)
        {
            try
            {
                var dto = courseId == 0 ?
                    await _db.SingleAsync<Video, VideoDTO>(s => s.Id.Equals(id), include) :
                    await _db.SingleAsync<Video, VideoDTO>(m =>  m.CourseId.Equals(courseId) && 
                        m.ModuleId.Equals(moduleId) &&  m.Id.Equals(id) , include);
                if (dto == null) return NotFound();
                return dto;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Database Failure");
            }
        }
        [HttpPost]
        public async Task<ActionResult<VideoDTO>> Post(int courseId, int moduleId, VideoDTO model)
        {
            try
            {
                if (courseId.Equals(0)) courseId = model.CourseId;
                if (moduleId.Equals(0)) moduleId = model.ModuleId;
                if (model == null) return BadRequest("No entity provided");
                if (!model.CourseId.Equals(courseId)) return BadRequest("Differing ids");
                if (!ModelState.IsValid) return BadRequest("Title is required");
                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(courseId));
                if (!exists)
                    return NotFound("Could not find related entity");
                exists = await _db.AnyAsync<Module>(a => a.Id.Equals(moduleId) &&
                    a.CourseId.Equals(courseId));
                if (!exists) return NotFound("Could not find related entity");
                var id = await _db.CreateAsync<VideoDTO, Video>(model);
                if (id < 1) return BadRequest("Unable to add the entity");
                var dto = await _db.SingleAsync<Video, VideoDTO>(s =>
                        s.CourseId.Equals(courseId) && s.ModuleId.Equals(moduleId)&& s.Id.Equals(id), true);
                if (dto == null) return BadRequest("Unable to add the entity");
                var uri = _linkGenerator.GetPathByAction("Get", "Videos",
                    new { courseId, moduleId, id });
                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Failed to add the entity");
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int courseId, int moduleId, int id, VideoDTO model)
        {
            try
            {
                if (model == null) return BadRequest("Missing entity");
                if (!id.Equals(model.Id)) return BadRequest("Differing ids");
                if (model.Title.IsNullOrEmptyOrWhiteSpace()) return BadRequest("Title is required");
                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(courseId));
                if (!exists)
                    return NotFound("Could not find related entity");
                exists = await _db.AnyAsync<Module>(a => a.Id.Equals(moduleId) 
                    && a.CourseId.Equals(courseId));
                if (!exists) return NotFound("Could not find related entity");
                exists = await _db.AnyAsync<Video>(a => a.Id.Equals(id));
                if (!exists) return NotFound("Could not find entity");
                if (await _db.UpdateAsync<VideoDTO, Video>(model))
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
        public async Task<IActionResult> Delete(int courseId, int moduleId, int id)
        {
            try
            {
                var exists = await _db.AnyAsync<Video>(a => a.CourseId.Equals(courseId) 
                    && a.ModuleId.Equals(moduleId) && a.Id.Equals(id));
                if (!exists) return BadRequest("Could not find entity");
                if (await _db.DeleteAsync<Video>(d => d.Id.Equals(id) && d.CourseId.Equals(courseId)
                    && d.ModuleId.Equals(moduleId)))   return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Failed to delete the entity");
            }
            return BadRequest("Failed to delete the entity");
        }
        #endregion
    }
}