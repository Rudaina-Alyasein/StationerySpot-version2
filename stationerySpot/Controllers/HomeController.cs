using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;


namespace stationerySpot.Controllers
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

        public IActionResult pressure(int id)
        {
            // نمرر القيمة للـ View
            ViewBag.LibraryId = id;

            // تحميل المنتجات المرتبطة بالمكتبة
            var products = _context.Products
                .Include(p => p.Category)     // تحميل الكاتيجوري
                .Include(p => p.Library)      // تحميل معلومات المكتبة (اختياري)
                .Where(p => p.LibraryId == id)
                .ToList();

            // تحقق إذا كانت هناك منتجات لهذه المكتبة
            if (!products.Any())
            {
                // إذا لم توجد منتجات، إظهار رسالة للمستخدم
                ViewBag.Message = "No products available for this library.";
                return View("NoProducts"); // عرض صفحة أو View مع رسالة
            }

            // تعيين اسم المكتبة من أول منتج
            ViewBag.LibraryName = products.First().Library.Name;
            return View(products); // تمرير المنتجات إلى الـ View
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


    }
}
