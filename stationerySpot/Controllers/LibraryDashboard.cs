using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;
using stationerySpot.ViewModels;
using System.Diagnostics;

namespace stationerySpot.Controllers
{
    public class LibraryDashboard : Controller
    {
        private readonly MyDbContext _context;
        public LibraryDashboard(MyDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            // جلب LibraryId من الجلسة
            int libraryId = HttpContext.Session.GetInt32("LibraryId").GetValueOrDefault();
            ViewBag.LibraryName = HttpContext.Session.GetString("LibraryName"); // جلب اسم المكتبة من الجلسة
            ViewBag.LibraryLogo = HttpContext.Session.GetString("LibraryLogo"); // جلب شعار المكتبة

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

            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }

        public IActionResult Orders()
        {
            return View();
        }
     
        public IActionResult profile()
        {
            int libraryId = HttpContext.Session.GetInt32("LibraryId").GetValueOrDefault();

            if (libraryId == 0)
            {
                return RedirectToAction("Login", "user");
            }

            // استخدام Include لتحميل بيانات حساب المكتبة المرتبطة
            var library =  _context.Libraries
                .Include(l => l.LibraryAccounts)  // تضمين بيانات الحساب
                .FirstOrDefault(l => l.Id == libraryId);

            if (library == null)
            {
                return NotFound();
            }

            return View(library);
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
            var Users = _context.Users.ToList();
            return View(Users);
        }

    }
}
