using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace stationerySpot.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public DbSet<CustomerInfo> CustomerInfos { get; set; }


    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ContactUsMessage> ContactUsMessages { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<Library> Libraries { get; set; }

    public virtual DbSet<LibraryAccount> LibraryAccounts { get; set; }

    public virtual DbSet<LibraryRegistrationRequest> LibraryRegistrationRequests { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<PrintRequest> PrintRequests { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ReviewsProduct> ReviewsProducts { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-DH1T2CV;Database=StationerySpot;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC072B0D11C1");

            entity.ToTable("Cart");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsCheckedOut).HasDefaultValue(false);

            modelBuilder.Entity<Cart>()
     .HasOne(d => d.User)  // العلاقة مع User
     .WithMany(p => p.Carts)  // User يمكن أن يمتلك عدة Carts
     .HasForeignKey(d => d.CustomerId)  // المفتاح الأجنبي الذي يشير إلى CustomerId
     .OnDelete(DeleteBehavior.ClientSetNull)  // عندما يتم حذف User، لا يتم حذف Cart
     .HasConstraintName("FK__Cart__CustomerId__1E6F845E");  // اسم القيد


        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC07721E6403");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.Cascade) // بدل ClientSetNull
                .HasConstraintName("FK__CartItems__CartI__22401542");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade) // بدل ClientSetNull
                .HasConstraintName("FK__CartItems__Produ__2334397B");
        });


        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07D05B03A0");

            entity.HasIndex(e => e.Name, "UQ__Categori__737584F657C73A96").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ContactUsMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactU__3214EC07C62DD71A");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FAQs__3214EC076911D5ED");

            entity.ToTable("FAQs");

            entity.Property(e => e.Answer).HasColumnType("text");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Question)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Library>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Librarie__3214EC0780B78CCB");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Location).HasColumnType("text");
            entity.Property(e => e.LogoPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("WebsiteURL");
            entity.Property(e => e.WorkingHours)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LibraryAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LibraryA__3214EC078BA540D6");

            entity.HasIndex(e => e.Email, "UQ__LibraryA__A9D1053431877A8B").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);

            entity.HasOne(d => d.Library).WithMany(p => p.LibraryAccounts)
                .HasForeignKey(d => d.LibraryId)
                .HasConstraintName("FK_LibraryAccounts_Libraries");
        });

        modelBuilder.Entity<LibraryRegistrationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LibraryR__3214EC07843BEF2D");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LibraryName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Location).HasColumnType("text");
            entity.Property(e => e.LogoPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Services).HasColumnType("text");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("WebsiteURL");
            entity.Property(e => e.WorkingHours)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07A7BBE6CE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Customer__4D94879B");

            entity.HasOne(d => d.Library).WithMany(p => p.Orders)
                .HasForeignKey(d => d.LibraryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__LibraryI__4E88ABD4");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC07FA907AEA");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__52593CB8");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<PrintRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PrintReq__3214EC073D1603F7");

            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Copies).HasDefaultValue(1);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.PrintSide)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Customer).WithMany(p => p.PrintRequests)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PrintRequ__Custo__0B91BA14");

            entity.HasOne(d => d.Library).WithMany(p => p.PrintRequests)
                .HasForeignKey(d => d.LibraryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PrintRequ__Libra__0C85DE4D");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07E57F5FF0");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Products__Catego__3E1D39E1");

            entity.HasOne(d => d.Library).WithMany(p => p.Products)
                .HasForeignKey(d => d.LibraryId)
                .HasConstraintName("FK__Products__Librar__4316F928");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3214EC072F847FB9");

            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Library).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.LibraryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__Library__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__UserId__693CA210");
        });

        modelBuilder.Entity<ReviewsProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReviewsP__3214EC070759943A");

            entity.ToTable("ReviewsProduct");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.ReviewsProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReviewsPr__Produ__0C50D423");

            entity.HasOne(d => d.User).WithMany(p => p.ReviewsProducts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReviewsPr__UserI__0D44F85C");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__3214EC073232CFBF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Library).WithMany(p => p.Services)
                .HasForeignKey(d => d.LibraryId)
                .HasConstraintName("FK__Services__Librar__208CD6FA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07230112E2");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105347FFC41AB").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
        modelBuilder.Entity<CustomerInfo>()
            .HasOne(c => c.Order)
            .WithMany()  // يمكنك تحديد العلاقة هنا حسب متطلباتك
            .HasForeignKey(c => c.OrderId);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
