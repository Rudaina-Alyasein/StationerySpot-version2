using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;
using System.Collections.Generic;
namespace stationerySpot.Controllers
{
    public class ChatController : Controller
    {
        private readonly MyDbContext _context;

        public ChatController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Chat(int libraryId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "User");
            }

            var messages = _context.Messages
                .Where(m => m.LibraryId == libraryId && m.UserId == Convert.ToInt32(userIdStr))
                .OrderBy(m => m.Timestamp)
                .ToList();

            ViewBag.LibraryId = libraryId;
            return View(messages);
        }

        [HttpPost]
        public IActionResult Send(int libraryId, string content)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["ReviewMessage"] = "Please log in first to add products to your cart..";
                return RedirectToAction("Login", "User");
            }
            var message = new Message
            {
                LibraryId = libraryId,
                UserId = Convert.ToInt32(userIdStr),
                Sender = "User",
                Content = content,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Chat", new { libraryId });
        }

    }
}

