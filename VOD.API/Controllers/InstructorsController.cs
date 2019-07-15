﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Services.IAdminService;
using VOD.Common.Entities;

namespace VOD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IAdminService _db;

        public InstructorsController(LinkGenerator linkGenerator, IAdminService db)
        {
            _linkGenerator = linkGenerator;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<InstructorDTO>>> Get(bool include = false)
        {
            try
            {
                return await _db.GetAsync<Instructor, InstructorDTO>(include);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<InstructorDTO>> Get(int id, bool include = false)
        {
            try
            {
                var dto = await _db.SingleAsync<Instructor, InstructorDTO>(i => i.Id.Equals(id), include);
                if (dto == null) return NotFound();
                return dto;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<InstructorDTO>> Post(InstructorDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No Entity Provided");
                var id = await _db.CreateAsync<InstructorDTO, Instructor>(model);
                var dto = await _db.SingleAsync<Instructor, InstructorDTO>(i=>i.Id.Equals(id));
                if (dto == null) return BadRequest("Unable to add the entity");
                var uri = _linkGenerator.GetPathByAction("Get", "Instructors", new { id});
                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to add the entity");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, InstructorDTO model)
        {
            try
            {
                if (!id.Equals(model.Id)) return BadRequest("Differing ids");
                var exists = await _db.AnyAsync<Instructor>(e => e.Id.Equals(id));
                if (!exists) return NotFound("Could not find entity");
                if(await _db.UpdateAsync<InstructorDTO, Instructor>(model)) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to update the entity");
            }
            return BadRequest("Unable to update the entity");
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var exists = await _db.AnyAsync<Instructor>(e => e.Id.Equals(id));
                if (!exists) return BadRequest("Could not find entity");
                if (await _db.DeleteAsync<Instructor>(d => d.Id.Equals(id)))
                    return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Failed to delete the entity");
            }
            return BadRequest("Failed to delete the entity");
        }


    }
}