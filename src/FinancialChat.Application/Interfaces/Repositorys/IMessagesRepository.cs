using FinancialChat.Application.Entities.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Interfaces.Repositorys
{
    public interface IMessagesRepository
    {
        void Add(MessagesData message);
        IEnumerable<MessagesData> GetChatMessages(string from, string to);
    }
}
