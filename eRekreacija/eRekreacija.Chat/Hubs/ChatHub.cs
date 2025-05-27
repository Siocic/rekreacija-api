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
        if (!int.TryParse(Context.UserIdentifier, out int userId))
        {
            throw new HubException("Invalid user ID");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
    }

    public async Task SendMessage(int recipientId, string content)
    {
        if (!int.TryParse(Context.UserIdentifier, out int senderId))
        {
            throw new HubException("Invalid user ID");
        }
        var messageToSave = new ChatMessageDTO
        {
            SenderId = senderId,
            RecipientId = recipientId,
            Content = content
        };
        var message = await _chatService.Insert(messageToSave);

        await Clients.Group($"user-{recipientId}").SendAsync("ReceiveMessage", message);
    }
}

