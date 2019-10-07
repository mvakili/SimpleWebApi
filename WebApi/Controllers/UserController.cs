using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using AutoMapper;
using WebApi.ViewModels.UserModels;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using WebApi.Exceptions.UserExceptions;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Services.IUserService _userService;

        public UserController(Services.IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginViewModel input)
        {
            try
            {
                var token = _userService.Authenticate(input);
                return Ok(token);
            }
            catch (UserNotFoundException)
            {
                return BadRequest("Username or password is incorrect");
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.RegisterAsync(input);
            return Ok();
        }

        [HttpGet("profile")]
        public IActionResult GetAuthenticatedUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    var user = _userService.GetUser(Guid.Parse(identity.FindFirst(ClaimTypes.Name).Value));
                    return Ok(user);
                } else
                {
                    return BadRequest("Invalid parameter");
                }
            }
            catch (UserNotFoundException)
            {
                return BadRequest();
            }

        }
    }
}
