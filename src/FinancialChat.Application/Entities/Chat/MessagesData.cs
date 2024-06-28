namespace FinancialChat.Application.Entities.Chat
{
    public class MessagesData
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }

        public override string ToString()
        {
            return $"From: {From} To: {To} Message: {Message} Created:{Created}";
        }
    }
}
