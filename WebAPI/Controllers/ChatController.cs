using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Model.ViewModels.UserModels;
using System.Security.Claims;
using Model.Exceptions.UserExceptions;
using Business.Services;
using Model.ViewModels.Chat;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

       

        [HttpPost]
        public async Task<IActionResult> SendMessageAsync([FromBody]SendMessageViewModel input)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                     await _chatService.SendMessageAsync(input, identity);
                    return Ok();
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
