using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;

namespace stationerySpot.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;
        private readonly EmailService _emailService;

        public AdminController(MyDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;

        }
        public async Task<IActionResult> Index()
        {
            var requests = await _context.LibraryRegistrationRequests
                .ToListAsync();

            return View(requests);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveLibrary(int id)
        {
            var request = await _context.LibraryRegistrationRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            //// تأكد من وجود المستخدم الذي تقدم بالطلب
            //var user = await _context.Users.FindAsync(request.Id); // استخدام OwnerId في الطلب، وليس Id
            //if (user == null)
            //{
            //    return NotFound("Owner not found");
            //}
            // إنشاء مكتبة جديدة في جدول Libraries مع جميع البيانات المطلوبة
            var library = new Library
            {
                Name = request.LibraryName,
                Location = request.Location ?? "Not Provided", // معالجة القيم الفارغة
                Description = request.Description ?? "No Description",
                Phone = request.Phone ?? "0000000000",
                LogoPath = request.LogoPath ?? "/default-logo.png",
                WorkingHours = request.WorkingHours ?? "9:00 AM - 5:00 PM",
                WebsiteUrl = request.WebsiteUrl ?? "N/A",
                City = request.City
            };

            _context.Libraries.Add(library);
            await _context.SaveChangesAsync();

            // إضافة الخدمات إلى جدول Services
            if (!string.IsNullOrEmpty(request.Services))
            {
                // افترض أن الخدمة هي قائمة خدمات مفصولة بفواصل أو فراغات، نحتاج إلى تقسيمها إلى خدمات فردية
                var services = request.Services.Split(','); // أو استخدم أي طريقة أخرى لتقسيم النص

                foreach (var serviceName in services)
                {
                    var service = new Service
                    {
                        LibraryId = library.Id,
                        Name = serviceName.Trim(), // التأكد من إزالة الفراغات الزائدة
                        Price = 0, // يمكن تعديل هذا حسب الحاجة
                        CreatedAt = DateTime.Now
                    };

                    _context.Services.Add(service);
                }
                await _context.SaveChangesAsync();
            }

            // توليد كلمة مرور عشوائية وتشفيرها
            var randomPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
            var hashedPassword = randomPassword;

            // إنشاء حساب للمكتبة وربطه بالمكتبة الجديدة
            var libraryAccount = new LibraryAccount
            {
                LibraryId = library.Id, // استخدام ID المكتبة التي أنشأناها
                Email = request.Email,
                PasswordHash = hashedPassword
            };

            _context.LibraryAccounts.Add(libraryAccount);

            // تحديث حالة الطلب إلى "مقبول"
            request.Status = "Approved";
            await _context.SaveChangesAsync();

            // إرسال البريد الإلكتروني الرسمي
            string emailBody = $"Dear {library.Name},\n\n" +
                               "Your library has been approved!\n\n" +
                               "Here are your login details:\n" +
                               $"📧 Email: {request.Email}\n" +
                               $"🔑 Password: {randomPassword}\n\n" +
                               "Please change your password after logging in.\n\n" +
                               "Best regards,\nStationery Spot Team";

            _emailService.SendEmail(request.Email, "Library Approval", emailBody);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> RejectLibrary(int id, string rejectionReason)
        {
            var request = await _context.LibraryRegistrationRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            // البحث عن الحساب المرتبط بالبريد الإلكتروني وحذفه
            var existingAccount = await _context.LibraryAccounts
                .FirstOrDefaultAsync(a => a.Email == request.Email);
            if (existingAccount != null)
            {
                _context.LibraryAccounts.Remove(existingAccount);
            }

            // البحث عن المكتبة المرتبطة بالطلب وحذفها
            var library = await _context.Libraries
                .FirstOrDefaultAsync(l => l.Name == request.LibraryName);
            if (library != null)
            {
                _context.Libraries.Remove(library);  // حذف المكتبة من الجدول
            }

            // تغيير حالة الطلب إلى "Rejected"
            request.Status = "Rejected";
            await _context.SaveChangesAsync();

            // التأكد من أن هناك سبب للرفض، وإلا سيتم وضع رسالة افتراضية
            string reasonMessage = !string.IsNullOrWhiteSpace(rejectionReason)
                ? rejectionReason
                : "We regret to inform you that your library registration request has been declined due to unmet requirements.";

            string emailBody = $"Dear {request.LibraryName},\n\n" +
                               "We appreciate your interest in joining our platform. " +
                               "Unfortunately, we are unable to approve your request at this time.\n\n" +
                               $"**Reason:** {reasonMessage}\n\n" +
                               "If you have any questions or wish to reapply with additional details, please feel free to contact us.\n\n" +
                               "Best regards,\nStationery Spot Team";

            // إرسال الإيميل بالرفض مع السبب
            _emailService.SendEmail(request.Email, "Library Registration Rejected", emailBody);

            return RedirectToAction("Index");  // إعادة التوجيه إلى صفحة القائمة بعد الرفض
        }



    }
}
