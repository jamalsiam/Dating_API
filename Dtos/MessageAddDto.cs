namespace Api.Dtos
{
    public class MessageAddDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Text { get; set; }
        public int Reaction { get; set; }
    }
}