namespace Api.Entities
{
    public class UserFollow
    {
        public AppUser Following { get; set; }
        public int FollowingId { get; set; }
        public AppUser Follower { get; set; }
        public int FollowerId { get; set; }

    }
}