using System;

namespace Api.Dtos
{
    public class PostLikeReadDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public string UserPhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool FollowingByAccount { get; set; }
    }
}