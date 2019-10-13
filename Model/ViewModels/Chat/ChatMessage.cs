using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.Chat
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual UserModels.UserViewModel From { get; set; }
        public virtual UserModels.UserViewModel To { get; set; }


    }
}
