namespace FinancialChat.Application.Entities.MessageModels
{
    public class MessageInputModel
    {
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
