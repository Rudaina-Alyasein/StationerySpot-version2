using Microsoft.EntityFrameworkCore;
using stationerySpot.Models;

var builder = WebApplication.CreateBuilder(args);

// إضافة خدمات البريد الإلكتروني
builder.Services.AddSingleton<EmailService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // مدة انتهاء الجلسة
    options.Cookie.HttpOnly = true; // تحسين الأمان
    options.Cookie.IsEssential = true; // التأكد من عمل الـ Session حتى مع سياسة الخصوصية
});


var app = builder.Build();



// تفعيل الـ Session
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
pattern: "{controller=user}/{action=Index}/{id?}");
//pattern: "{controller=Home}/{action=Cart}/{id?}");
//pattern: "{controller=LibraryDashboard}/{action=profile}/{id?}");




app.Run();
