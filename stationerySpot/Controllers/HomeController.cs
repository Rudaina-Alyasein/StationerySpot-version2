using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;
using stationerySpot.ViewModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;


namespace stationerySpot.ControllersF
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;


        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            var latestOffers = _context.Offers
                .Where(o => o.ExpiryDate == null || o.ExpiryDate > DateTime.Now)
                .OrderByDescending(o => o.Id)
                .Take(6)
                .Select(o => new OfferViewModel
                {
                    Id =o.Id,
                    Title = o.Title,
                    Description = o.Description ?? "",
                    ImagePath = o.ImagePath ?? "/images/default-offer.jpg",
                    LibraryName = o.Library.Name,
                    ExpiryDate = o.ExpiryDate
                })
                .ToList();

            return View(latestOffers); 
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
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return View(request);
            }

            if (request.LogoPathFile != null && request.LogoPathFile.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", request.LogoPathFile.FileName);
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.LogoPathFile.CopyToAsync(stream);
                }
                request.LogoPath = "/assets/images/" + request.LogoPathFile.FileName;
            }
            else
            {
                ModelState.AddModelError("LogoPathFile", "Please upload a logo image.");
                return View(request);
            }

            _context.LibraryRegistrationRequests.Add(request);
            await _context.SaveChangesAsync();

            // بدل Redirect
            TempData["SuccessMessage"] = "Your registration request has been submitted successfully. Please wait for approval. Best wishes!";

            return View(new LibraryRegistrationRequest());  // رجع View فاضي جديد
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
        public IActionResult Cart(int id)
        {
            return View();
        }
        public IActionResult Checkout()
        {
            // استرجاع الطلب المرتبط بحالة Pending
            var pendingOrder = _context.Orders
                .Where(o => o.Status == "Pending")
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault();

            // التحقق من وجود طلب مفتوح
            if (pendingOrder != null)
            {
                // استرجاع معلومات العميل بناءً على OrderId
                var customerInfo = _context.CustomerInfos
                    .FirstOrDefault(c => c.OrderId == pendingOrder.Id);

                // إنشاء ViewModel الجديد الذي يحتوي على كل من Order و CustomerInfo
                var viewModel = new OrderWithCustomerInfoViewModel
                {
                    Order = pendingOrder,
                    CustomerInfo = customerInfo
                };

                // حساب المجموع الكلي (Subtotal)
                viewModel.Order.TotalAmount = viewModel.Order.OrderDetails.Sum(od => od.Price * od.Quantity);

                // إرسال ViewModel إلى الـ View
                return View(viewModel);
            }

            // في حال لم يكن هناك طلب مفتوح، إعادة التوجيه لصفحة أخرى
            return RedirectToAction("Index");
        }

        public IActionResult Pressure(int id)
        {
            var library = _context.Libraries.FirstOrDefault(l => l.Id == id);

            if (library == null)
            {
                return NotFound(); // إذا لم يتم العثور على المكتبة
            }

            var products = _context.Products.Include(p => p.Category).Where(p => p.LibraryId == id).ToList();
            var reviews = _context.Reviews.Include(r => r.User).Where(r => r.LibraryId == id).ToList();
            var services = _context.Services.Where(s => s.LibraryId == id).ToList();
            var printRequests = _context.PrintRequests.Where(pr => pr.LibraryId == id).ToList();
            var libraryAccounts = _context.LibraryAccounts.Where(la => la.LibraryId == id).ToList();

            if (!products.Any())
            {
                ViewBag.Message = "No products available for this library.";
                return View("NoProducts");
            }

            // جلب الفئات الفريدة باستخدام GroupBy بناءً على الـ Id
            var categories = _context.Products
                                     .Select(p => p.Category)
                                     .GroupBy(c => c.Id)
                                     .Select(g => g.FirstOrDefault()) // اختيار أول فئة من كل مجموعة
                                     .ToList();

            var viewModel = new LibraryViewModel
            {
                Library = library,
                Products = products,
                Reviews = reviews,
                Services = services,
                PrintRequests = printRequests,
                Categories = categories // إرسال قائمة الفئات بدون تكرار
            };

            return View(viewModel);
        }


        public IActionResult FilterProducts(string category, string sort)
        {
            // جيبي كل المنتجات أولاً
            var products = _context.Products.AsQueryable();

            // فلترة حسب الكاتيجوري
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
            }

            // ترتيب حسب المطلوب
            switch (sort)
            {
                case "PriceLowToHigh":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "PriceHighToLow":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "Newest":
                    products = products.OrderByDescending(p => p.CreatedAt); // لازم يكون عندك تاريخ للإضافة
                    break;
              
            }

            return PartialView("_FilteredProductsPartial", products.ToList());
        }
        public IActionResult SearchProducts(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return PartialView("_FilteredProductsPartial", new List<Product>());
            }

            // البحث في المنتجات
            var products = _context.Products
                                   .Where(p => p.Name.Contains(query) || p.Description.Contains(query))  // تعديل الشرط بما يناسبك
                                   .ToList();

            return PartialView("_FilteredProductsPartial", products);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products
    .Include(p => p.Category)
    .Include(p => p.Library)
    .Include(p => p.ReviewsProducts)
        .ThenInclude(rp => rp.User) // هون تربط كل Review مع الـ User تبعه
    .FirstOrDefault(p => p.Id == id);


            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost]
        public IActionResult SendMessage(ContactUsMessage contactMessage)
        {
            if (!ModelState.IsValid)
            {
                return View("ContactUs");
            }

            contactMessage.SubmittedAt = DateTime.Now;
            //contactMessage.IsRead = false;

            _context.ContactUsMessages.Add(contactMessage);
            _context.SaveChanges();

            // إرسال البريد
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("rudainaalyasein@gmail.com", "Location Name");
                mail.To.Add("rudainaalyasein@gmail.com");
                mail.Subject = $"Message from {contactMessage.Name}";
                mail.Body = $"Name: {contactMessage.Name}\nEmail: {contactMessage.Email}\n\nMessage:\n{contactMessage.Message}";
                mail.ReplyToList.Add(new MailAddress(contactMessage.Email));

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("rudainaalyasein@gmail.com", "zbxs temj qnnu qqqv"),
                    EnableSsl = true
                };

                smtp.Send(mail);
                // إضافة رسالة نجاح باستخدام TempData
                TempData["SuccessMessage"] = "The message sent Successfuly!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("ContactUs");
        }
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["CartMessage"] = "Please log in first to add products to your cart..";
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdStr);

            // جلب أو إنشاء السلة للمستخدم
            var cart = _context.Carts.FirstOrDefault(c => c.CustomerId == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = userId, // 👈 ضروري تحطها هون
                    CreatedAt = DateTime.Now,
                    IsCheckedOut = false // ممكن تحطها false كبداية إذا بدك
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }


            // تحقق من وجود المنتج مسبقاً
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                });
            }

            _context.SaveChanges();
            TempData["CartMessage"] = "The product has been added to the cart.!";
            return RedirectToAction("Index");
        }

        public IActionResult Cart1(string couponCode)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login");
            }

            int userId = int.Parse(userIdStr);

            var cart = _context.Carts
                               .Include(c => c.CartItems)
                               .ThenInclude(ci => ci.Product)
                               .FirstOrDefault(c => c.CustomerId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                ViewBag.Message = "Your cart is empty!";
                return View(cart);
            }

            // حساب Subtotal
            var subtotal = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            // رسوم الشحن الثابتة
            decimal shipping = 3.00m;

            // خصم إذا تم إدخال كوبون
            decimal discount = 0;
            string appliedCoupon = null;

            if (!string.IsNullOrEmpty(couponCode) && couponCode.ToUpper() == "SAVE20")
            {
                appliedCoupon = "SAVE20";
                HttpContext.Session.SetString("AppliedCoupon", appliedCoupon);
                HttpContext.Session.SetString("CouponDiscount", "1.10");               // قيمة الخصم

            }
            else
            {
                HttpContext.Session.Remove("AppliedCoupon");
            }

            // تطبيق الخصم إن وجد
            var sessionCoupon = HttpContext.Session.GetString("AppliedCoupon");
            if (!string.IsNullOrEmpty(sessionCoupon) && sessionCoupon == "SAVE20")
            {
                discount = subtotal * 0.20m;
            }

            // الحساب النهائي
            var total = subtotal + shipping - discount;

            // تمرير البيانات إلى View
            ViewBag.Subtotal = subtotal;
            ViewBag.Shipping = shipping;
            ViewBag.Discount = discount;
            ViewBag.Total = total;
            ViewBag.AppliedCoupon = sessionCoupon;

            return View(cart);
        }



        [HttpPost]
        public IActionResult UpdateCart([FromBody] UpdateCartRequest request)
        {
            if (request == null || request.UpdatedCartItems == null)
            {
                return Json(new { success = false, message = "Update data not received." });
            }

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Json(new { success = false, message = "You must log in first." });
            }

            int userId = int.Parse(userIdStr);

            var cart = _context.Carts
                               .Include(c => c.CartItems)
                               .FirstOrDefault(c => c.CustomerId == userId);

            if (cart == null)
            {
                return Json(new { success = false, message = "Cart not found." });
            }

            foreach (var updatedItem in request.UpdatedCartItems)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == updatedItem.ProductId);
                if (cartItem != null)
                {
                    cartItem.Quantity = updatedItem.Quantity;
                }
            }

            _context.SaveChanges();

            return Json(new { success = true });
        }
        public IActionResult RemoveFromCart(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Account"); // إعادة توجيه للمستخدم إذا لم يكن مسجلاً
            }

            int userId = int.Parse(userIdStr);

            var cart = _context.Carts
                               .Include(c => c.CartItems)
                               .FirstOrDefault(c => c.CustomerId == userId);

            if (cart == null)
            {
                return RedirectToAction("Index", "Cart"); // العودة إلى صفحة السلة في حال عدم وجود السلة
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == id);
            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart1"); // العودة إلى السلة بعد الحذف
        }

        public IActionResult CheckoutProcess()
        {
            // التأكد من أن المستخدم قام بتسجيل الدخول
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login");
            }

            int userId = int.Parse(userIdStr);

            // جلب سلة التسوق للمستخدم
            var cart = _context.Carts
                                .Include(c => c.CartItems)
                                .ThenInclude(ci => ci.Product)
                                .FirstOrDefault(c => c.CustomerId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                ViewBag.Message = "Your cart is empty!";
                return View();
            }

            // حساب الـ Subtotal، Shipping، Discount و Total
            var subtotal = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            var shipping = 3.00m; // رسوم الشحن الثابتة
            var discount = 0.00m; // الخصم الافتراضي

            // جلب الكوبون من الـ Session (إذا كان موجود)
            var appliedCoupon = HttpContext.Session.GetString("AppliedCoupon");
            if (!string.IsNullOrEmpty(appliedCoupon) && appliedCoupon.ToUpper() == "SAVE20")
            {
                discount = subtotal * 0.20m; // تطبيق الخصم 20%
            }

            // حساب الـ Total بعد تطبيق الخصم
            var total = subtotal + shipping - discount;

            // تحديد المكتبات المختلفة بناءً على المنتجات
            var libraryIds = cart.CartItems.Select(ci => ci.Product.LibraryId).Distinct().ToList();

            if (libraryIds.Count == 0)
            {
                ViewBag.Message = "Unable to determine libraries for the order.";
                return View();
            }

            // إنشاء طلب لكل مكتبة
            foreach (var libraryId in libraryIds)
            {
                var order = new Order
                {
                    CustomerId = userId,
                    LibraryId = libraryId, // مكتبة مختلفة لكل طلب
                    TotalAmount = total,
                    Status = "Pending", // يمكنك تخصيص حالة الطلب
                    CreatedAt = DateTime.Now
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                // إضافة العناصر إلى جدول OrderDetail للطلب الحالي
                foreach (var item in cart.CartItems.Where(ci => ci.Product.LibraryId == libraryId))
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    };

                    _context.OrderDetails.Add(orderDetail);
                }

                _context.SaveChanges(); // حفظ البيانات في قاعدة البيانات
            }

            // إفراغ سلة التسوق بعد إتمام الطلب
            cart.CartItems.Clear();
            _context.SaveChanges();

            // إعادة التوجيه إلى صفحة تأكيد أو الدفع أو غيرها
            return RedirectToAction("Checkout");
        }

        [HttpPost]
        public async Task<IActionResult>PlaceOrder(int orderId, string fullName, string email, string phone, string city, string streetAddress, string paymentMethod)
        {
            // استرجاع الطلب
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // تحديث حالة الطلب إلى "تم"
            order.Status = "Processing";
            // يمكنك تغيير الحالة إلى ما يناسبك
            _context.Update(order);
            await _context.SaveChangesAsync();

            // التحقق من وجود بيانات العميل
            var customerInfo = await _context.CustomerInfos
                .FirstOrDefaultAsync(c => c.OrderId == orderId);

            if (customerInfo == null)
            {
                // إذا لم تكن هناك بيانات للعميل، نقوم بإنشاء سجل جديد
                customerInfo = new CustomerInfo
                {
                    OrderId = orderId,
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    City = city,
                    StreetAddress = streetAddress,
                    PaymentMethod = paymentMethod
                };

                _context.CustomerInfos.Add(customerInfo);
            }
            else
            {
                // إذا كانت البيانات موجودة، نقوم بتحديثها
                customerInfo.FullName = fullName;
                customerInfo.Email = email;
                customerInfo.Phone = phone;
                customerInfo.City = city;
                customerInfo.StreetAddress = streetAddress;
                customerInfo.PaymentMethod = paymentMethod;
            }

            await _context.SaveChangesAsync();

            // إرسال رسالة نجاح عبر TempData
            TempData["SuccessMessage"] = "Your order has been placed successfully!";

            // إعادة التوجيه إلى صفحة تأكيد الطلب
            return RedirectToAction("OrderConfirmation");
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }

        //Review Library Section 
        [HttpPost]
        public IActionResult AddReview(int rating, string comment, int libraryId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["ReviewMessage"] = "Please log in first to add products to your cart..";
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdStr);
          
                var newReview = new Review
                {
                    UserId = userId,
                    LibraryId = libraryId,
                    Rating = rating,
                    Comment = comment,
                    CreatedAt = DateTime.Now
                };
                _context.Reviews.Add(newReview);
            

            _context.SaveChanges();

            return Json(new { success = true });
        }
        //pdf library section 
        [HttpPost]
        public async Task<IActionResult> SubmitPrintRequest(IFormFile pdfFile, int copies, string color, string printSide, string? message, int libraryId)
        {
            if (pdfFile == null || pdfFile.Length == 0 || !pdfFile.FileName.EndsWith(".pdf"))
            {
                return Json(new { success = false, message = "Invalid PDF file." });
            }

            // احفظ الملف
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(pdfFile.FileName)}";
            var filePath = Path.Combine("wwwroot/uploads/pdfs", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            // احصل على ID المستخدم الحالي من الجلسة
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            int userId = int.Parse(userIdStr);
            // خزّن البيانات في قاعدة البيانات
            var printRequest = new PrintRequest
            {
                CustomerId = userId,
                LibraryId = libraryId,
                DocumentPath = $"/uploads/pdfs/{fileName}",
                Copies = copies,
                Color = color,
                PrintSide = printSide,
                Message = message,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.PrintRequests.Add(printRequest);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Print request submitted successfully!" });
        }
        public IActionResult AcademicProject()
        {
            return View();
        }
        public IActionResult Projects()
        {
            var projects = _context.Projects.Include(p => p.Library).ToList();
            return View(projects);
        }
        public IActionResult Offer(int id)
        {
            var offer = _context.Offers
                                .Where(o => o.Id == id)
                                .Select(o => new OfferViewModel
                                {
                                    Id = o.Id,  // إضافة الـ Id للعرض
                                    Title = o.Title,
                                    Description = o.Description ?? "",
                                    ImagePath = o.ImagePath ?? "/images/default-offer.jpg",
                                    Link = o.Link ?? "#",
                                    LibraryName = o.Library.Name,
                                    ExpiryDate = o.ExpiryDate,
                                    // إضافة التعليقات الخاصة بالعرض
                                    Comments = o.OfferComments.Select(c => new CommentViewModel
                                    {
                                        UserName = c.User.Name,  // افترض أن `User` هو الكائن الذي يحتوي على اسم المستخدم
                                        UserImage = string.IsNullOrEmpty(c.User.ProfileImagePath) ? "/images/default-user.jpg" : c.User.ProfileImagePath,
                                        CommentText = c.CommentText,
                                        DatePosted = c.DatePosted
                                    }).ToList()
                                })
                                .FirstOrDefault();

            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);  // تمرير الـ OfferViewModel إلى الـ View
        }

        [HttpPost]
        //comment for offer 
        public IActionResult AddComment(int OfferId, string CommentText)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId.IsNullOrEmpty())
            {
                TempData["Error"] = "You must be logged in to leave a comment.";
                return RedirectToAction("Offer", new { id = OfferId });
            }

            var offer = _context.Offers.FirstOrDefault(o => o.Id == OfferId);
            if (offer == null)
            {
                TempData["Error"] = "Offer not found.";
                return RedirectToAction("Index");  
            }

            if (string.IsNullOrEmpty(CommentText))
            {
                TempData["Error"] = "Comment cannot be empty.";
                return RedirectToAction("Offer", new { id = OfferId });
            }

            var comment = new OfferComment
            {
                OfferId = OfferId,
                CommentText = CommentText,
                UserId = Convert.ToInt32(userId),
                DatePosted = DateTime.Now
            };

            _context.OfferComments.Add(comment);
            _context.SaveChanges();

            // إعادة التوجيه لصفحة العرض مع إضافة تعليقات جديدة
            return RedirectToAction("Offer", new { id = OfferId });
        }

    }
}
