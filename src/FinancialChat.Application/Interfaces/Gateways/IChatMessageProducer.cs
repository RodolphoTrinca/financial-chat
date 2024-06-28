using FinancialChat.Application.Entities.Chat;

public interface IChatMessageProducer {
    bool SaveUserMessage(MessagesData messageModel);
}