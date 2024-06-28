using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Entities.MessageModels
{
    public class StockMessageModel
    {
        public string StockTicker { get; set; } = string.Empty;
        public string Requester { get; set; } = string.Empty;
    }
}
