using Microsoft.AspNetCore.Mvc;
using stationerySpot.Models;
using System.Collections.Generic;
namespace stationerySpot.Controllers
{
    public class chatController : Controller
    {
        private static List<ChatMessageViewModel> Messages = new List<ChatMessageViewModel>();

        [HttpGet]
        public IActionResult GetMessages(string userId, string libraryId)
        {
            var chatMessages = Messages
                .Where(m => (m.SenderId.ToString() == userId && m.ReceiverId.ToString() == libraryId) ||
                            (m.SenderId.ToString() == libraryId && m.ReceiverId.ToString() == userId))
                .OrderBy(m => m.Timestamp)
                .ToList();

            return Json(chatMessages);
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] ChatMessageViewModel message)
        {
            message.Timestamp = DateTime.Now;
            Messages.Add(message);
            return Json(new { success = true });
        }
    }
    }

