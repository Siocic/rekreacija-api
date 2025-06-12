using eRekreacija.Services.Interfaces;
using eRekreacija.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    public class ChatController : BaseCRUDController<ChatMessageDTO, ChatMessageDTO, object>
    {
        private readonly IChatService _chatService;
        private readonly ILogger<AppointmentController> _logger;

        public ChatController(IChatService chatService, ILogger<AppointmentController> logger) : base(chatService)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetChatHistory(string user1Id, string user2Id)
        {
            var messages = await _chatService.GetMessagesAsync(user1Id, user2Id);
            return Ok(messages);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("conversations/{userId}")]
        public async Task<IActionResult> GetUserConversations(string userId, bool hall = true)
        {
            var conversations = await _chatService.GetUserConversationsAsync(userId, hall);
            return Ok(conversations);
        }

    }
}
