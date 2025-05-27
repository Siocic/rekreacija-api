using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
public interface IChatService : ICRUDService<ChatMessageDTO, ChatMessageDTO, ChatMessageDTO>
{
    Task<IEnumerable<ChatMessageDTO>> GetMessagesAsync(int user1Id, int user2Id);
}

