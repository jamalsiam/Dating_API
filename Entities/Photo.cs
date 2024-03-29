using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } 
        public int? PostId { get; set; }

        [ForeignKey(nameof(CommentId))]
        public PostComment Comment { get; set; } 
        public int? CommentId { get; set; }



    }
}