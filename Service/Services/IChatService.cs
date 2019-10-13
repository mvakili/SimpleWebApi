using Model.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IChatService
    {
        Task SendMessageAsync(SendMessageViewModel message, ClaimsIdentity identity);

    }
}
