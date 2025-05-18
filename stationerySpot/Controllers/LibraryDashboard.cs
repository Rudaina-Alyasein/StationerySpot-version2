using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;
using stationerySpot.Services;
using stationerySpot.ViewModels;
using System.Diagnostics;
using stationerySpot.DTO;


namespace stationerySpot.Controllers
{
    public class LibraryDashboard : BaseController
    {
        private readonly MyDbContext _context;
        private readonly OrderService _orderService;
       
        public IActionResult Index()
        {
            int? libraryId = HttpContext.Session.GetInt32("LibraryId");

            if (libraryId == null) return RedirectToAction("Login");

            var requests = _context.PrintRequests
                .Where(r => r.LibraryId == libraryId)
                .ToList();


            var categoryData = _context.Products
                .Where(p => p.LibraryId == libraryId && p.Category != null)
                .GroupBy(p => p.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                }).ToList();

            var orders = _context.Orders.Where(o => o.LibraryId == libraryId);

            var viewModel = new LibraryStatsViewModel
            {
                // بيانات الريكوستس

                NewRequests = _context.PrintRequests.Count(p => p.LibraryId == libraryId && p.Status == "new"),
                InProgressRequests = _context.PrintRequests.Count(p => p.LibraryId == libraryId && p.Status == "Pending"),
                DoneRequests = _context.PrintRequests.Count(p => p.LibraryId == libraryId && p.Status == "Completed"),
                CancelledRequests = _context.PrintRequests.Count(p => p.LibraryId == libraryId && p.Status == "cancelled"),

                // بيانات المنتجات حسب الفئة
                CategoryNames = categoryData.Select(c => c.Category!).ToList(),
                ProductCounts = categoryData.Select(c => c.Count).ToList(),
                 // ✅ Orders data
    TotalOrders = orders.Count(),
                PendingOrders = orders.Count(o => o.Status == "Pending"),
                CompletedOrders = orders.Count(o => o.Status == "Completed"),
                CancelledOrders = orders.Count(o => o.Status == "Cancelled"),
                TotalSalesAmount = orders.Sum(o => o.TotalAmount)
            };

            return View(viewModel);
        }

        public LibraryDashboard(MyDbContext context, OrderService orderService)
        {
            _context = context;
            _orderService = orderService;


        }
        public IActionResult ProductsManagement()
        {
            // جلب LibraryId من الجلسة
            int libraryId = HttpContext.Session.GetInt32("LibraryId").GetValueOrDefault();
           

            // إذا لم يكن هناك LibraryId في الجلسة، يمكنك توجيه المستخدم إلى صفحة تسجيل الدخول
            if (libraryId == 0)
            {
                return RedirectToAction("Login", "User");
            }

            // جلب المنتجات التي تخص المكتبة بناءً على LibraryId
            var products = _context.Products
                                   .Where(p => p.LibraryId == libraryId)
                                   .Include(p => p.Category)
                                   .ToList();

            // جلب قائمة الفئات (Categories)
            var categories = _context.Categories
                                     .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                                     .ToList();

            // إعداد الـ ViewModel لتمريره للـ View
            var viewModel = new ProductViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(IFormCollection formData)
        {
            string imagePath = null;

            // استخدم "NewProduct.Name" لأنك تستخدم asp-for في النموذج الفرعي
            string name = formData["NewProduct.Name"];
            string description = formData["NewProduct.Description"];
            decimal price = Convert.ToDecimal(formData["NewProduct.Price"]);
            int stock = Convert.ToInt32(formData["NewProduct.Stock"]);

            // استخراج LibraryId من الجلسة
            int libraryId = HttpContext.Session.GetInt32("LibraryId").GetValueOrDefault();

            // إذا لم يكن LibraryId موجودًا في الجلسة، يمكنك إظهار رسالة خطأ أو توجيه المستخدم إلى تسجيل الدخول
            if (libraryId == 0)
            {
                return RedirectToAction("Login", "User");
            }

            int categoryId = Convert.ToInt32(formData["NewProduct.CategoryId"]);

            if (formData.Files.Count > 0)
            {
                var file = formData.Files["NewProduct.ImagePath"];

                if (file != null && file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    imagePath = $"/uploads/{fileName}";
                }
            }

            var newProduct = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                ImagePath = imagePath,
                LibraryId = libraryId,  // ربط المنتج بحساب المكتبة
                CategoryId = categoryId,
                CreatedAt = DateTime.Now
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            TempData["Message"] = "The product has been added successfully.";

            return RedirectToAction("ProductsManagement");
        }
        // GET: /Product/GetProductById
        public IActionResult GetProductById(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Json(product); // Return the product data as JSON
        }

        // POST: /Product/UpdateProduct
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(IFormCollection formData)
        {
            try
            {
                int productId = Convert.ToInt32(formData["Id"]);
                var product = await _context.Products.FindAsync(productId);

                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found." });
                }

                // تحديث البيانات النصية
                product.Name = formData["Name"];
                product.Description = formData["Description"];
                product.Price = Convert.ToDecimal(formData["Price"]);
                product.Stock = Convert.ToInt32(formData["Stock"]);
                product.CategoryId = Convert.ToInt32(formData["NewProduct.CategoryId"]);

                // تحديث الصورة إذا تم رفع صورة جديدة
                if (formData.Files.Count > 0)
                {
                    var file = formData.Files["ImageFile"];
                    if (file != null && file.Length > 0)
                    {
                        // حذف الصورة القديمة إذا كانت موجودة
                        if (!string.IsNullOrEmpty(product.ImagePath))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // حفظ الصورة الجديدة بنفس الطريقة المتبعة في الإضافة
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        product.ImagePath = $"/uploads/{fileName}";
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Product updated successfully." });
                }
                catch (DbUpdateException dbEx)
                {
                    var innerException = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                    return Json(new { success = false, message = $"Database error: {innerException}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            // البحث عن المنتج باستخدام الـ productId
            var product = await _context.Products.FindAsync(productId);

            // إذا لم يتم العثور على المنتج، قم بإظهار رسالة خطأ أو توجيه المستخدم إلى صفحة أخرى
            if (product == null)
            {
                return NotFound();
            }

            // حذف المنتج
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Product deleted successfully.";

            return RedirectToAction("ProductsManagement");
        }

        public IActionResult Orders(string status, string sortOrder = "asc", string search = "")
        {
            int libraryId = HttpContext.Session.GetInt32("LibraryId").GetValueOrDefault();

            if (libraryId == 0)
            {
                return RedirectToAction("Login", "User");
            }

            var orderViewModels = _orderService.GetOrders(libraryId, status, sortOrder, search);

            ViewData["SelectedStatus"] = status;
            ViewData["SortOrder"] = sortOrder;
            ViewData["SearchQuery"] = search;

            return View(orderViewModels);
        }




        public IActionResult profile()
        {
            int libraryId = HttpContext.Session.GetInt32("LibraryId").GetValueOrDefault();

            if (libraryId == 0)
            {
                return RedirectToAction("Login", "user");
            }

            var library =  _context.Libraries
                .Include(l => l.LibraryAccounts) 
                .FirstOrDefault(l => l.Id == libraryId);

            if (library == null)
            {
                return NotFound();
            }


            var vm = new LibraryDashboardViewModel
            {
                Library = library,
                ChangePassword = new ChangePasswordLibraryViewModel() 
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordLibraryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ChangePasswordModal", model); // رجعي نفس المودال
            }

            int libraryId = HttpContext.Session.GetInt32("LibraryId").GetValueOrDefault();
            var library = _context.Libraries
                                  .Include(l => l.LibraryAccounts)
                                  .FirstOrDefault(l => l.Id == libraryId);

            var user = library?.LibraryAccounts.FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return PartialView("_ChangePasswordModal", model);
            }

            if (!VerifyPassword(model.PasswordHash, user.PasswordHash))
            {
                ModelState.AddModelError(nameof(model.PasswordHash), "Current password is incorrect.");
                return PartialView("_ChangePasswordModal", model);
            }

            user.PasswordHash = model.NewPassword;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("Profile");
        }

        // دوال مساعدة للتشفير والتحقق (حسب طريقة تخزينك)
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            // هنا ضع طريقة التحقق من كلمة السر حسب التشفير المستخدم
            return inputPassword == storedHash; // مثال مبسط جداً (غير آمن)
        }

      

        [HttpPost]
        public async Task<IActionResult> EditProfile(Library LibraryInfo, IFormFile LogoPathFile)
        {
            int libraryId = HttpContext.Session.GetInt32("LibraryId").GetValueOrDefault();

            if (libraryId == 0)
            {
                return RedirectToAction("Login", "User");
            }

            var library = await _context.Libraries.FindAsync(libraryId);
            if (library == null)
            {
                return NotFound();
            }

            // تحديث القيم
            library.Name = LibraryInfo.Name;
            library.Location = LibraryInfo.Location;
            library.Description = LibraryInfo.Description;
            library.Phone = LibraryInfo.Phone;
            library.WorkingHours = LibraryInfo.WorkingHours;
            library.WebsiteUrl = LibraryInfo.WebsiteUrl;
            library.City = LibraryInfo.City;

            // التعامل مع رفع الشعار الجديد
            if (LogoPathFile != null && LogoPathFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(LogoPathFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await LogoPathFile.CopyToAsync(stream);
                }

                // تحديث مسار الشعار
                library.LogoPath = "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Library profile updated successfully!";
            return RedirectToAction("Profile");
        }

      

        public IActionResult chat()
        {
            int libraryId = HttpContext.Session.GetInt32("LibraryId") ?? 0;

            // جيب كل المستخدمين اللي أرسلوا رسائل لهاي المكتبة
            var users = _context.Messages
                .Where(m => m.LibraryId == libraryId)
                .Select(m => m.User)
                .Distinct()
                .ToList();

            return View(users); // هذا اللي إنتي عملتي له @model List<User>
        }
        [HttpGet]
        public IActionResult GetMessages(int userId)
        {
            int libraryId = HttpContext.Session.GetInt32("LibraryId") ?? 0;

            var messages = _context.Messages
                .Where(m => m.LibraryId == libraryId && m.UserId == userId)
                .OrderBy(m => m.Timestamp)
                .Select(m => new
                {
                    SenderType = m.Sender, // "User" أو "Admin"
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    SenderName = m.Sender == "User"
                        ? _context.Users.Where(u => u.Id == m.UserId).Select(u => u.Name).FirstOrDefault()
                        : "Admin"
                })
                .ToList();

            return Json(messages);
        }
        [HttpPost]
        public IActionResult SendMessage([FromBody] MessageDto messageDto)
        {
            int libraryId = HttpContext.Session.GetInt32("LibraryId") ?? 0;

            var message = new Message
            {
                LibraryId = libraryId,
                UserId = messageDto.ReceiverId,
                Sender = "Admin",
                Content = messageDto.Message,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, string newStatus)
        {
            // جيبي الطلب من قاعدة البيانات
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction("LibraryOrders");
            }

            // عدلي الحالة
            order.Status = newStatus;
            order.CreatedAt = DateTime.Now; // اختياري: لو عندك عمود تاريخ التحديث

            // خزني التغييرات
            _context.SaveChanges();

            TempData["Success"] = $"Order #{orderId} marked as {newStatus}.";
            return RedirectToAction("Orders");
        }

        public IActionResult PdfRequests()
        {
            var libraryId = HttpContext.Session.GetInt32("LibraryId");

            if (libraryId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var requests = _context.PrintRequests
                .Where(r => r.LibraryId == libraryId)
                .Include(r => r.Customer)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return View(requests);
        }
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var request = _context.PrintRequests.Find(id);
            if (request != null)
            {
                request.Status = status;
                _context.SaveChanges();
            }
            TempData["StatusMessage"] = "Status updated successfully!";

            return RedirectToAction("PdfRequests");
        }
      
        public IActionResult ReviewsLibrary()
        {
            int? libraryId = HttpContext.Session.GetInt32("LibraryId");
            if (libraryId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var reviews = _context.Reviews
                .Include(r => r.User)
                .Where(r => r.LibraryId == libraryId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return View(reviews);
        }
        [HttpPost]
        public IActionResult ReplyToReview(int id, string reply)
        {
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                review.Reply = reply;
                review.Status = "Approved"; 
                review.CreatedAt = DateTime.Now;
                _context.SaveChanges();
            }

            return RedirectToAction("ReviewsLibrary"); 
        }

        [HttpPost]
        public IActionResult RejectReview(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                review.Status = "Rejected"; 
                _context.SaveChanges();
            }

            return RedirectToAction("ReviewsLibrary"); 
        }


    }
}
