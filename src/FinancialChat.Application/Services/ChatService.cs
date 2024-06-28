using FinancialChat.Application.Entities.Chat;
using FinancialChat.Application.Interfaces.Repositorys;
using FinancialChat.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly ILogger<ChatService> _logger;
        private readonly IMessagesRepository _repository;

        public ChatService(ILogger<ChatService> logger, IMessagesRepository messagesRepository)
        {
            _logger = logger;
            _repository = messagesRepository;
        }

        public IEnumerable<MessagesData> GetChatRoomMessages(string chatRoom)
        {
            return _repository.GetChatRoomMessages(chatRoom);
        }

        public void SaveMessage(MessagesData message)
        {
            _repository.Add(message);
        }
    }
}
