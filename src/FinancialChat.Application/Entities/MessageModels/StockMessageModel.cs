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

        public override bool Equals(object? obj)
        {
            return obj is StockMessageModel model &&
                   StockTicker == model.StockTicker &&
                   Requester == model.Requester;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StockTicker, Requester);
        }
    }
}
