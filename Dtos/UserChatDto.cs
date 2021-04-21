using System;

namespace Api.Dtos
{
    public class UserChatDto
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public string Fullname { get; set; }
        public string PhotoUrl { get; set; }
        public string TextMessage { get; set; }
        public bool Seen { get; set; }
        public DateTime MessageCreateAt { get; set; }
        public bool Active { get; set; }

    }
}