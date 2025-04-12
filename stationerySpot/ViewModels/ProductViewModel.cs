using stationerySpot.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace stationerySpot.ViewModels
{
    public class ProductViewModel
    {
        public List<Product>? Products { get; set; } = new List<Product>(); // لعرض المنتجات في الجدول
        public Product? NewProduct { get; set; } = new Product(); // للمنتج الجديد في النموذج
        public List<SelectListItem>? Categories { get; set; } = new List<SelectListItem>(); // لعرض الفئات
        public int? SelectedCategoryId { get; set; } // الفئة المختارة عند إضافة منتج جديد
    }
}
