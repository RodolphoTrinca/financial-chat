using FinancialChat.Application.Entities.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Application.SignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static Dictionary<string, UserConnection> _usersConnected = new Dictionary<string, UserConnection>();
        private static Dictionary<string, string> _userConnectionId = new Dictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            _userConnectionId[name] = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        public async Task JoinSpecificChatRoom(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

            _usersConnected[Context.ConnectionId] = connection;

            await Clients.Group(connection.ChatRoom).SendAsync("JoinSpecificChatRoom", "admin", $"{connection.Username} has joined {connection.ChatRoom}");
        }

        public async Task SendMessage(string message)
        {
            if (_usersConnected.TryGetValue(Context.ConnectionId, out var connection))
            {
                await Clients.Group(connection.ChatRoom)
                    .SendAsync("ReceiveSpecificMessage", connection.Username, message);
            }
        }

        public async Task SendMessageUserAsync(string from, string to, string message){
            await Clients.User(to).SendAsync("ReceiveSpecificMessage", from, message);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;
            _userConnectionId.Remove(name);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
