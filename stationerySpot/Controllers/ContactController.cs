using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stationerySpot.ControllersF;
using stationerySpot.Models;
using System.Net.Mail;
using System.Net;

namespace stationerySpot.Controllers
{
    public class ContactController : Controller
    {
        private readonly MyDbContext _context;
        public ContactController( MyDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendMessageJson([FromBody] ContactUsMessage contactMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill all required fields correctly.");
            }

            contactMessage.SubmittedAt = DateTime.Now;

            _context.ContactUsMessages.Add(contactMessage);
            _context.SaveChanges();

            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress("rudainaalyasein@gmail.com", "Location Name"),
                    Subject = $"Message from {contactMessage.Name}",
                    Body = $"Name: {contactMessage.Name}\nEmail: {contactMessage.Email}\n\nMessage:\n{contactMessage.Message}"
                };
                mail.To.Add("rudainaalyasein@gmail.com");
                mail.ReplyToList.Add(new MailAddress(contactMessage.Email));

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("rudainaalyasein@gmail.com", "zbxs temj qnnu qqqv"),
                    EnableSsl = true
                };

                smtp.Send(mail);

                return Ok(new { success = true, message = "Message sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to send email.",
                    error = ex.ToString() 
                });
            }

        }


    }
}
