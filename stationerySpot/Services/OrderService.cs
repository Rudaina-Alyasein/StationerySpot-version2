using stationerySpot.Models;
using stationerySpot.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace stationerySpot.Services
{
    public class OrderService
    {
        private readonly MyDbContext _context;

        public OrderService(MyDbContext context)
        {
            _context = context;
        }

        public List<OrderViewModel> GetOrders(int libraryId, string status = null, string sortOrder = "asc", string search = "")
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.LibraryId == libraryId);

            // فلترة حسب الـ status إذا كان موجودًا
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(o => o.Status == status);
            }

            // فلترة حسب الـ search إذا كان موجودًا
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.Id.ToString().Contains(search) || o.User.Name.Contains(search));
            }

            // ترتيب البيانات بناءً على sortOrder
            if (sortOrder == "asc")
            {
                query = query.OrderBy(o => o.CreatedAt); // ترتيب من الأقدم إلى الأحدث
            }
            else
            {
                query = query.OrderByDescending(o => o.CreatedAt); // ترتيب من الأحدث إلى الأقدم
            }

            var orders = query.ToList();

            var orderViewModels = orders.Select(o => new OrderViewModel
            {
                OrderId = o.Id,
                OrderDate = o.CreatedAt ?? DateTime.MinValue,
                Status = o.Status ?? "Unknown",
                TotalAmount = o.TotalAmount,
                Name = o.User?.Name ?? "N/A",
                Email = o.User?.Email ?? "N/A",
                Products = o.OrderDetails.Select(od => new ProductInfo
                {
                    ProductName = od.Product?.Name ?? "N/A",
                    Quantity = od.Quantity,
                    UnitPrice = od.Price
                }).ToList()
            }).ToList();

            return orderViewModels;
        }


    }

}
