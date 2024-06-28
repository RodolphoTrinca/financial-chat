using FinancialChat.Application.Entities.Chat;
using FinancialChat.Application.Interfaces.Repositorys;
using FinancialChat.Infra.Context;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Infra.Repository
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly ILogger<MessagesRepository> _logger;
        private readonly MessagesDbContext _context;

        public MessagesRepository(ILogger<MessagesRepository> logger, MessagesDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Add(MessagesData message)
        {
            _logger.LogDebug($"Saving message... {message}");
            _context.Add(message);

            _context.SaveChanges();
        }

        public IEnumerable<MessagesData> GetChatRoomMessages(string chatRoom)
        {
            _logger.LogDebug($"Retriving messages from chat room: {chatRoom}");
            return _context.Messages
                .Where(m => m.To == chatRoom)
                .OrderByDescending(m => m.Created)
                .Take(50)
                .ToList();
        }
    }
}
