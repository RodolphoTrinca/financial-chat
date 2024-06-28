namespace FinancialChat.Application.Entities.Configuration.RabbitMQ
{
    public class RabbitMQQueueNames
    {
        public string StockPriceRequest { get; set; }
        public string ChatMessages { get; set; }
        public string HubConnectionMessages { get; set; }
    }
}
