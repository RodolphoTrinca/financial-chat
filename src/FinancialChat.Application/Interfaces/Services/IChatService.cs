using FinancialChat.Application.Entities.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Interfaces.Services
{
    public interface IChatService
    {
        void SendMessage(MessagesData message);

        IEnumerable<MessagesData> GetChatMessage(string from, string to);
    }
}
