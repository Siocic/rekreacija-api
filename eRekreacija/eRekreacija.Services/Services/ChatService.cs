using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Services;
using Microsoft.EntityFrameworkCore;

public class ChatService : BaseCRUDService<tbl_ChatMessage, ChatMessageDTO, ChatMessageDTO, object>, IChatService
{
    private readonly RekreacijaContext _rekreacijaContext;

    public ChatService(RekreacijaContext rekreacijaContext, IMapper mapper) : base(rekreacijaContext, mapper) {
        _rekreacijaContext = rekreacijaContext;
    }

    public async Task<IEnumerable<ChatMessageDTO>> GetMessagesAsync(string user1Id, string user2Id)
    {
        var messages = await _rekreacijaContext.TblChatMessages
            .Where(m =>
                (m.SenderId == user1Id && m.RecipientId == user2Id) ||
                (m.SenderId == user2Id && m.RecipientId == user1Id))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ChatMessageDTO>>(messages);
    }
    public async Task<List<UserConversationDTO>> GetUserConversationsAsync(string userId)
    {
        var messages = await _rekreacijaContext.TblChatMessages
            .Where(m => m.SenderId == userId || m.RecipientId == userId)
            .OrderByDescending(m => m.Timestamp)
            .ToListAsync();

        var conversationUserIds = messages
            .Select(m => m.SenderId == userId ? m.RecipientId : m.SenderId)
            .Distinct()
            .ToList();

        // Fetch all halls whose user_id matches any conversation user
        var halls = await _rekreacijaContext.TblObject
            .Where(o => conversationUserIds.Contains(o.user_id))
            .ToDictionaryAsync(o => o.user_id, o => o.name);

        var conversations = messages
            .GroupBy(m => m.SenderId == userId ? m.RecipientId : m.SenderId)
            .Select(g =>
            {
                var otherUserId = g.Key;
                var lastMessage = g.First();

                halls.TryGetValue(otherUserId, out var hallName); // if not found, hallName will be null

                return new UserConversationDTO
                {
                    ConversationUserId = otherUserId,
                    LastMessage = lastMessage.Content,
                    LastTimestamp = lastMessage.Timestamp,
                    HallName = hallName ?? "Unknown Hall"
                };
            })
            .OrderByDescending(c => c.LastTimestamp)
            .ToList();

        return conversations;
    }


}
