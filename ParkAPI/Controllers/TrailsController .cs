using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkAPI.Models;
using ParkAPI.Models.Dtos;
using ParkAPI.Repository.IRepository;

namespace ParkAPI.Controllers
{
    [Route("api/Trails")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of trails.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(List<TrailDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTrails()
        {
            var objList = _trailRepository.GetTrails();
            
            var objDto = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            
            return Ok(objDto);
        }

        /// <summary>
        /// Get specific trail.
        /// </summary>
        /// <param name="trailId"> Id of trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepository.GetTrail((trailId));

            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<TrailDto>(obj);

            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateTrail([FromBody] TrailDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("","Trail already exists.");
                return StatusCode(404, ModelState);
            }

            var obj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.CreateTrail(obj))
            {
                ModelState.AddModelError("",$"Something went wrong when saving the record {obj.Name}");
                return StatusCode(500,ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = obj.Id}, obj);
        }
        
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var obj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.UpdateTrail(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }

            var obj = _trailRepository.GetTrail(trailId);

            if (!_trailRepository.DeleteTrail(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
