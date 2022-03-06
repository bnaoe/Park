using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ParkAPI.Models;
using ParkAPI.Repository.IRepository;

namespace ParkAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepository.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or Password is is incorrect" });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            bool userNameUnique = _userRepository.IsUniqueUser(model.Username);

            if (!userNameUnique)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            var user = _userRepository.Register(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering." });
            }

            return Ok();
        }

    }
}
