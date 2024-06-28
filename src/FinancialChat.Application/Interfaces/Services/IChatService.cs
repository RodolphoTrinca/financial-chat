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
        void SaveMessage(MessagesData message);

        IEnumerable<MessagesData> GetChatRoomMessages(string chatroom);
    }
}
