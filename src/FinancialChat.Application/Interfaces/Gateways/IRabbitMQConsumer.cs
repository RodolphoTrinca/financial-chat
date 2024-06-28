using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Interfaces.Gateways
{
    public interface IRabbitMQConsumer
    {
        Task ReadMessgaes();
    }
}
