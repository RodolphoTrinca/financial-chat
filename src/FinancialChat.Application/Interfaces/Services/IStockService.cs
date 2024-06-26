namespace FinancialChat.Application.Interfaces.Services
{
    public interface IStockService
    {
        bool GetStockPrice(string stockTicker);
    }
}