using Microsoft.AspNetCore.SignalR;
public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

    }
    public async Task RegisterUser(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
    }
    public async Task SendMessage(string senderId, string recipientId, string content)
    {
        var messageToSave = new ChatMessageDTO
        {
            SenderId = senderId,
            RecipientId = recipientId,
            Content = content
        };

        var message = await _chatService.Insert(messageToSave);

        await Clients.Group($"user-{recipientId}").SendAsync("ReceiveMessage", message);
        await Clients.Group($"user-{senderId}").SendAsync("ReceiveMessage", message);
    }

}

