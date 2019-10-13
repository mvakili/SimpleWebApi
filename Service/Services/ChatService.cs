using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.SignalRHubs;
using DataAccess.Repositories;
using Microsoft.AspNetCore.SignalR;
using Model.Entities;
using Model.ViewModels.Chat;
using Model.ViewModels.UserModels;

namespace Business.Services
{
    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ChatService(IHubContext<ChatHub> chatHub, IMapper mapper, IUserService userService)
        {
            this._chatHub = chatHub;
            this._mapper = mapper;
            this._userService = userService;
        }
 
        public async Task SendMessageAsync(SendMessageViewModel input, ClaimsIdentity identity)
        {
            var fromUser = _userService.GetIdentityUser(identity);
            var toUser = _userService.GetUserByUsername(input.Username);
            var message = new ChatMessage()
            {
                CreatedAt = DateTime.Now,
                From = fromUser,
                To = toUser,
                Id = Guid.NewGuid(),
                Text = input.Text
            };

            await _chatHub.Clients.Users(fromUser.Id.ToString(), toUser.Id.ToString()).SendCoreAsync("chatMessages", new ChatMessage[] { message });
        }
    }
}
