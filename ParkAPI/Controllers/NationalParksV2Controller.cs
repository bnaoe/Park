using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkAPI.Models;
using ParkAPI.Models.Dtos;
using ParkAPI.Repository.IRepository;

namespace ParkAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
    public class NationalParksV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(List<NationalParkDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalParks()
        {
            var obj = _nationalParkRepository.GetNationalParks().FirstOrDefault();
            
            var objDto = new List<NationalParkDto>();
            
            return Ok(_mapper.Map<NationalParkDto>(obj));
        }



    }
}
