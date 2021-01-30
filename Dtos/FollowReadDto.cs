namespace Api.Dtos
{
    public class FollowReadDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; } 
        public int FollowingId { get; set; } 
        public int FollowerId { get; set; } 
        public string PhotoUrl { get; set; }
        public bool IsFollowByAccount { get; set; }
        

    }
}