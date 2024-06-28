using FinancialChat.Application.Entities.MessageModels;
using FinancialChat.Application.Entities.StockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Interfaces.Services
{
    public interface IStockService
    {
        Task<StockData?> GetStockPriceAsync(StockMessageModel stockMessage);
    }
}
