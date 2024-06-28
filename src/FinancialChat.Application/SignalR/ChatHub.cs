using FinancialChat.Application.Entities.Chat;
using FinancialChat.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialChat.Application.SignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static Dictionary<string, UserConnection> _usersConnected = new Dictionary<string, UserConnection>();
        private static Dictionary<string, string> _userConnectionId = new Dictionary<string, string>();
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            _userConnectionId[name] = Context.ConnectionId;

            await Groups.AddToGroupAsync(Context.ConnectionId, name);

            await base.OnConnectedAsync();
        }

        public async Task JoinSpecificChatRoom(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

            _usersConnected[Context.ConnectionId] = connection;

            var messages = _chatService.GetChatRoomMessages(connection.ChatRoom);
            foreach (var message in messages)
            {
                await Clients.Group(connection.ChatRoom).SendAsync("ReceiveSpecificMessage", message.From, message.Message);
            }

            await Clients.Group(connection.ChatRoom).SendAsync("JoinSpecificChatRoom", "admin", $"{connection.Username} has joined {connection.ChatRoom}");
        }

        public async Task SendMessage(string message)
        {
            if (_usersConnected.TryGetValue(Context.ConnectionId, out var connection))
            {
                var messageData = new MessagesData()
                {
                    From = connection.Username,
                    To = connection.ChatRoom,
                    Message = message
                };

                _chatService.SaveMessage(messageData);

                await Clients.Group(connection.ChatRoom)
                    .SendAsync("ReceiveSpecificMessage", connection.Username, message);
            }
        }

        public async Task SendMessageUserAsync(string from, string to, string message){
            await Clients.User(to).SendAsync("ReceiveSpecificMessage", from, message);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;
            _userConnectionId.Remove(name);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, name);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
