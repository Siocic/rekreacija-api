using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
public interface IChatService : ICRUDService<ChatMessageDTO, ChatMessageDTO, object>
{
    Task<IEnumerable<ChatMessageDTO>> GetMessagesAsync(string user1Id, string user2Id);
    Task<List<UserConversationDTO>> GetUserConversationsAsync(string userId, bool hall);

}

