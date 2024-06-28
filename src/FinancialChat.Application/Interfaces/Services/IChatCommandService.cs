namespace FinancialChat.Application.Interfaces.Services
{
    public interface IChatCommandService
    {
        bool SendMessageWithStockPrice(string stockTicker, string username);
    }
}