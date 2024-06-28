using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Entities.Configuration.Gateways
{
    public class StockPriceConfig
    {
        public string Url { get; set; }
        public string Format { get; set; }
        public string F { get; set; }
        public string Path { get; set; }
    }
}
