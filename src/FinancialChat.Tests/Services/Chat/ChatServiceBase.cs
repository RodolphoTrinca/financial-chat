using FinancialChat.Application.Interfaces.Repositorys;
using FinancialChat.Application.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;


namespace FinancialChat.Tests.Services.Chat
{
    public class ChatServiceBase
    {
        protected ILogger<ChatService> _logger;
        protected IMessagesRepository _repository;
        protected ChatService _service;

        public ChatServiceBase()
        {
            _logger = Substitute.For<ILogger<ChatService>>();
            _repository = Substitute.For<IMessagesRepository>();

            _service = new ChatService(_logger, _repository);
        }
    }
}
