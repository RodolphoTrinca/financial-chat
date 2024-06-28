using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Entities.StockData
{
    public class StockData
    {
        public string Symbol { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Volume { get; set; }

        public override string ToString()
        {
            return $"Symbol: {Symbol}, Date: {Date}, Time: {Time}, " +
                $"Open: {Open}, Close: {Close}, Low: {Low}, " +
                $"High: {High}, Volume: {Volume}";
        }
    }
}
