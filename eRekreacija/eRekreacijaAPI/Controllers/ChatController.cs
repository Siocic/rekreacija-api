using eRekreacija.Services.Interfaces;
using eRekreacija.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eRekreacijaAPI.Controllers
{
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetChatHistory(int user1Id, int user2Id)
        {
            var messages = await _chatService.GetMessagesAsync(user1Id, user2Id);
            return Ok(messages);
        }

    }
}
