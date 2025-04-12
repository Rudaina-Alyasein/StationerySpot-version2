using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;

namespace stationerySpot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;


        public HomeController(ILogger<HomeController> logger,MyDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Libraries()
        {
            var libraries = _context.Libraries.ToList();

            return View(libraries);
        }
        public IActionResult Library()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
       
        // POST: Register Library
        [HttpPost]
        public async Task<IActionResult> Register(LibraryRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                // إضافة الأخطاء إلى ModelState بشكل مباشر
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return View(request);  // في حال وجود أخطاء، قم بإرجاع نفس الصفحة مع الأخطاء
            }

            // التحقق إذا كانت الصورة موجودة في الحقل
            if (request.LogoPathFile != null && request.LogoPathFile.Length > 0)
            {
                // تحديد مسار حفظ الصورة على الخادم (ضمن مجلد wwwroot/images)
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", request.LogoPathFile.FileName);

                // التأكد من وجود المجلد، إذا لم يكن موجودًا، أنشئه
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // حفظ الصورة على الخادم
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.LogoPathFile.CopyToAsync(stream);
                }

                // حفظ المسار النسبي للصورة في الحقل LogoPath
                request.LogoPath = "/assets/images/" + request.LogoPathFile.FileName;
            }
            else
            {
                // إضافة رسالة خطأ إذا لم يتم رفع صورة
                ModelState.AddModelError("LogoPathFile", "Please upload a logo image.");
                return View(request);  // إعادة نفس الصفحة مع الرسالة
            }

            // إضافة البيانات إلى قاعدة البيانات
            _context.LibraryRegistrationRequests.Add(request);
            await _context.SaveChangesAsync();

            return RedirectToAction("Register");  // إعادة التوجيه إلى صفحة التسجيل بعد النجاح
        }

        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult TermsConditions()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
       public IActionResult pressure(int id )
        {
            // نمرر القيمة للـ View
            ViewBag.LibraryId = id;

       
            return View();
        }


    }
}
