using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly IUserLocationService _userLocationService;

        public LocationController(IUserLocationService service)
        {
            this._userLocationService = service;
        }

        [HttpPost]
        public async Task<IActionResult> SetUserLocationAsync(LocationViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                await _userLocationService.SetUserLocationAsync(input, identity);
                return Ok();
            }
            else
            {
                return BadRequest("Invalid parameter");
            }
        }
        [HttpGet]
        public IActionResult GetUserLocation()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var location = _userLocationService.GetUserLocationModel(identity);
                return Ok(location);
            }
            else
            {
                return BadRequest("Invalid parameter");
            }
        }
        [HttpGet("around")]
        public IActionResult GetUsersAround([FromQuery] double x, [FromQuery] double y, [FromQuery] double radius)
        {
            var users = _userLocationService.GetUsersAround(x, y, radius);
            return Ok(users);
        }
        [HttpGet("around/me")]
        public IActionResult GetUsersAroundMe([FromQuery] double radius)
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var location = _userLocationService.GetUsersAroundMe(identity, radius);
                return Ok(location);
            }
            else
            {
                return BadRequest("Invalid parameter");
            }
        }
    }
}
