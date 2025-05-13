using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using stationerySpot.ViewModels;
using Microsoft.AspNetCore.Identity;

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
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "User"); 
        }
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                TempData["WarningMessage"] = "Please log in first.";
                return RedirectToAction("Login", "User");
            }

            if (!int.TryParse(userId, out int id))
            {
                TempData["WarningMessage"] = "Invalid user data.";
                return RedirectToAction("Login", "User");
            }

            var user = _context.Users
                                .Include(u => u.Address)       // تضمين العنوان
                                .Include(u => u.Orders)        // تضمين الطلبات
                                .Include(u => u.PaymentMethod) // تضمين طريقة الدفع
                                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                TempData["WarningMessage"] = "User not found.";
                return RedirectToAction("Login", "User");
            }

            // بناء الـ ViewModel
            var model = new ProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                ProfileImagePath = user.ProfileImagePath,

                CreatedAt = user.CreatedAt,
                Address = user.Address,            // تضمين العنوان
                Orders = user.Orders.Select(o => new OrderViewModel
                {
                    OrderId = o.Id,
                    OrderDate = o.CreatedAt ?? DateTime.Now,
                    TotalAmount = o.TotalAmount
                }).ToList(),                      // تضمين الطلبات
                PaymentMethod = user.PaymentMethod  // تضمين طريقة الدفع
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult UpdatePaymentMethod(string paymentMethod)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (!int.TryParse(userId, out int id))
            {
                return RedirectToAction("Login", "User");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var paymentMethodRecord = _context.PaymentMethods.FirstOrDefault(p => p.UserId == id);

            if (paymentMethodRecord != null)
            {
                // تحديث طريقة الدفع
                paymentMethodRecord.MethodName = paymentMethod;
                paymentMethodRecord.IsPaid = paymentMethod == "cod" ? true : false; // إذا كانت COD يعتبر الدفع تم
                paymentMethodRecord.PaymentDate = DateTime.Now;
            }
            else
            {
                var newPaymentMethod = new PaymentMethod
                {
                    UserId = id,
                    MethodName = paymentMethod,
                    IsPaid = paymentMethod == "cod" ? true : false,
                    PaymentDate = DateTime.Now
                };

                _context.PaymentMethods.Add(newPaymentMethod);
            }

            _context.SaveChanges(); // حفظ التغييرات في قاعدة البيانات

            return RedirectToAction("Profile"); // إعادة التوجيه إلى صفحة طرق الدفع
        }
        [HttpPost]
        public IActionResult UpdateAddress(string street, string city, string postalCode, string notes)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "User");

            var address = _context.Addresses.FirstOrDefault(a => a.UserId == userId);
            if (address == null)
            {
                address = new Address { UserId = userId };
                _context.Addresses.Add(address);
            }

            address.Street = street;
            address.City = city;
            address.PostalCode = postalCode;
            address.Notes = notes;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Your address has been updated successfully!";

            return RedirectToAction("Profile");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAccountDetails(ProfileViewModel model)
        {
            // التأكد من أن المستخدم موجود
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "User");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return RedirectToAction("Login", "User");

            // التحقق من كلمة السر الحالية إذا كانت صحيحة (بدون Hashing)
            if (user.PasswordHash != model.CurrentPassword)
            {
                ModelState.AddModelError(string.Empty, "Incorrect current password.");
                return View(model);
            }

            // تحديث البيانات
            user.Name = $"{model.FirstName} {model.LastName}";
            user.Email = model.Email;

            // إذا تم إدخال كلمة سر جديدة، حدثها
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                user.PasswordHash = model.NewPassword; // ملاحظة: غير آمن تخزينها بهذا الشكل
            }

            // حفظ التعديلات
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Account updated successfully!";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileImage(IFormFile ProfileImage)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "User");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return RedirectToAction("Login", "User");

            // تحقق إذا كان يوجد صورة تم رفعها
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // تخزين الصورة في مكان ما
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", ProfileImage.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                // تحديث مسار الصورة في قاعدة البيانات
                user.ProfileImagePath = "/uploads/" + ProfileImage.FileName;
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Profile image updated successfully!";
            return RedirectToAction("Profile"); // إعادة التوجيه إلى صفحة الـ Profile بعد التحديث
        }

    }

}
