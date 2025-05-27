using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Services;
using Microsoft.EntityFrameworkCore;

public class ChatService : BaseCRUDService<tbl_ChatMessage, ChatMessageDTO, ChatMessageDTO, ChatMessageDTO>, IChatService
{
    private readonly RekreacijaContext _rekreacijaContext;

    public ChatService(RekreacijaContext rekreacijaContext, IMapper mapper) : base(rekreacijaContext, mapper) { }

    public async Task<IEnumerable<ChatMessageDTO>> GetMessagesAsync(int user1Id, int user2Id)
    {
        var messages = await _rekreacijaContext.TblChatMessages
            .Where(m =>
                (m.SenderId == user1Id && m.RecipientId == user2Id) ||
                (m.SenderId == user2Id && m.RecipientId == user1Id))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ChatMessageDTO>>(messages);
    }
}
