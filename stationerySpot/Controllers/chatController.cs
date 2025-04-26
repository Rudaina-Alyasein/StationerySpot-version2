using Microsoft.AspNetCore.Mvc;
using stationerySpot.Models;
using System.Collections.Generic;
namespace stationerySpot.Controllers
{
    public class ChatController : Controller
    {
        private static List<ChatMessageViewModel> Messages = new List<ChatMessageViewModel>();

        [HttpGet]
        public IActionResult GetMessages(int libraryId)
        {
            // استرجاع الـ userId من الجلسة
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized(); // في حال عدم وجود المستخدم في الجلسة
            }

            var chatMessages = Messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == libraryId) ||
                            (m.SenderId == libraryId && m.ReceiverId == userId))
                .OrderBy(m => m.Timestamp)
                .ToList();

            return Json(chatMessages);
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] ChatMessageViewModel message)
        {
            // استرجاع الـ userId من الجلسة
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized(); // في حال عدم وجود المستخدم في الجلسة
            }

            message.SenderId = userId.Value; // تعيين SenderId من الجلسة
            message.Timestamp = DateTime.Now;
            Messages.Add(message);

            return Json(new { success = true });
        }
        [HttpGet]
        public IActionResult GetMessagesUser(int userId)
        {
            // استرجاع الـ userId من الجلسة
            var libraryId = HttpContext.Session.GetInt32("LibraryId");

            if (userId == null)
            {
                return Unauthorized(); // في حال عدم وجود المستخدم في الجلسة
            }

            var chatMessages = Messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == libraryId) ||
                            (m.SenderId == libraryId && m.ReceiverId == userId))
                .OrderBy(m => m.Timestamp)
                .ToList();

            return Json(chatMessages);
        }
    }
}

