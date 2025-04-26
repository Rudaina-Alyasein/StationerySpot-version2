using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace stationerySpot.Controllers
{
    public class userController : Controller
    {
        private readonly MyDbContext _context;
        public userController(MyDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View(); }
            [HttpPost]
            public IActionResult SignUp(string firstName, string lastName, string email, string password, string confirmPassword)
            {
                // تحقق من وجود نفس الإيميل
                if (_context.Users.Any(u => u.Email == email))
                {
                    TempData["ErrorMessage"] = "Email is already registered."; // تخزين الخطأ في TempData
                    TempData["FirstName"] = firstName; // تخزين القيمة المدخلة في TempData
                    TempData["LastName"] = lastName;
                    TempData["Email"] = email;
                    return RedirectToAction("Index"); // إعادة التوجيه للصفحة نفسها
                }

                // تحقق من تطابق كلمة المرور
                if (password != confirmPassword)
                {
                    TempData["ErrorMessage"] = "Passwords do not match."; // تخزين الخطأ في TempData
                    TempData["FirstName"] = firstName; // تخزين القيم المدخلة
                    TempData["LastName"] = lastName;
                    TempData["Email"] = email;
                    return RedirectToAction("Index"); // إعادة التوجيه للصفحة نفسها
                }

                // تحقق من قوة كلمة المرور
                if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"))
                {
                    TempData["ErrorMessage"] = "Password must be at least 8 characters long and contain both letters and numbers."; // تخزين الخطأ في TempData
                    TempData["FirstName"] = firstName; // تخزين القيم المدخلة
                    TempData["LastName"] = lastName;
                    TempData["Email"] = email;
                    return RedirectToAction("Index"); // إعادة التوجيه للصفحة نفسها
                }

                // إذا كانت البيانات صحيحة، إنشاء المستخدم
                var user = new User
                {
                    Name = $"{firstName} {lastName}",
                    Email = email,
                    PasswordHash = password, // تشفير كلمة المرور
                    Role = "Customer", // تعيين دور المستخدم
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");  // إعادة التوجيه لصفحة الـIndex أو الصفحة التي تختارها
            }
       
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {
            // أولاً تحقق من الـ LibraryAccounts
            var libraryAccount = _context.LibraryAccounts.FirstOrDefault(l => l.Email == email && l.PasswordHash == password);
            if (libraryAccount != null)
            {
                // تخزين معلومات المكتبة في الجلسة
                HttpContext.Session.SetInt32("LibraryAccountId", libraryAccount.Id);  // ID تبع الحساب
                HttpContext.Session.SetInt32("LibraryId", libraryAccount.LibraryId);  // ID تبع المكتبة نفسها


                // جلب اسم المكتبة من جدول Libraries بشكل صحيح
                var library = _context.Libraries.FirstOrDefault(l => l.Id == libraryAccount.LibraryId);
                if (library != null)
                {
                    HttpContext.Session.SetString("LibraryName", library.Name);
                    HttpContext.Session.SetString("LibraryLogo", library.LogoPath);

                }

                HttpContext.Session.SetString("LibraryEmail", email); // تخزين الايميل في الجلسة

                return RedirectToAction("Index", "LibraryDashboard");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                ViewBag.Email = email;
                return RedirectToAction("Login");
            }

            // تخزين بيانات المستخدم في الـ Session
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", user.Name);

            // توجيه بناءً على الدور
            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else // مثلاً "Customer"
            {
                return RedirectToAction("Index", "Home");
            }

        }






        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // حذف جميع بيانات الجلسة
            return RedirectToAction("Login", "User"); // إعادة التوجيه لصفحة تسجيل الدخول
        }
        public IActionResult Profile()
        {
            // جلب بيانات المستخدم من الجلسة
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User"); // إذا لم تكن هناك بيانات في الجلسة، إعادة التوجيه إلى صفحة تسجيل الدخول
            }

            // تحويل UserId إلى نوع int باستخدام TryParse لتجنب الاستثناءات
            if (!int.TryParse(userId, out int id))
            {
                return RedirectToAction("Login", "User"); // إذا كانت القيمة غير صالحة، إعادة التوجيه إلى صفحة تسجيل الدخول
            }

            // جلب بيانات المستخدم من قاعدة البيانات
            var user = _context.Users
                               .Include(u => u.Orders)
                               .Include(u => u.PrintRequests)
                               .Include(u => u.Reviews)
                               .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return RedirectToAction("Login", "User"); // إذا لم يتم العثور على المستخدم، إعادة التوجيه إلى صفحة تسجيل الدخول
            }

            // إنشاء ViewModel مع البيانات
            var profileModel = new ProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                Orders = user.Orders.Select(order => new OrderViewModel
                {
                    Id = order.Id,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    CreatedAt = order.CreatedAt
                }).ToList(),
                // إذا كنت ترغب في إضافة PrintRequests و Reviews، قم بإلغاء تعليق الكود هنا
                //PrintRequests = user.PrintRequests.Select(request => new PrintRequestViewModel {...}).ToList(),
                //Reviews = user.Reviews.Select(review => new ReviewViewModel {...}).ToList()
            };

            // إرسال البيانات إلى الفيو
            return View(profileModel);
        }

        }
    }
