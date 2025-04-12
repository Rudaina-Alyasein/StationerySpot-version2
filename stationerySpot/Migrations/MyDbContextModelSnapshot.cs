﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using stationerySpot.Models;

#nullable disable

namespace stationerySpot.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("stationerySpot.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__Categori__3214EC07D05B03A0");

                    b.HasIndex(new[] { "Name" }, "UQ__Categori__737584F657C73A96")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("stationerySpot.Models.ContactU", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("LibraryId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int?>("SenderId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__ContactU__3214EC071CDE7C47");

                    b.HasIndex("LibraryId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("ContactUs");
                });

            modelBuilder.Entity("stationerySpot.Models.Faq", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__FAQs__3214EC076911D5ED");

                    b.ToTable("FAQs", (string)null);
                });

            modelBuilder.Entity("stationerySpot.Models.Library", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LogoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebsiteURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkingHours")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__Librarie__3214EC0780B78CCB");

                    b.HasIndex("OwnerId");

                    b.ToTable("Libraries");
                });

            modelBuilder.Entity("stationerySpot.Models.LibraryAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("LibraryId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__LibraryA__3214EC078BA540D6");

                    b.HasIndex("LibraryId");

                    b.HasIndex(new[] { "Email" }, "UQ__LibraryA__A9D1053431877A8B")
                        .IsUnique();

                    b.ToTable("LibraryAccounts");
                });

            modelBuilder.Entity("stationerySpot.Models.LibraryRegistrationRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LibraryName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LogoPath")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Services")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasDefaultValue("Pending");

                    b.Property<string>("WebsiteUrl")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("WebsiteURL");

                    b.Property<string>("WorkingHours")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__LibraryR__3214EC07843BEF2D");

                    b.ToTable("LibraryRegistrationRequests");
                });

            modelBuilder.Entity("stationerySpot.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("LibraryId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasDefaultValue("Pending");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("Id")
                        .HasName("PK__Orders__3214EC07A7BBE6CE");

                    b.HasIndex("CustomerId");

                    b.HasIndex("LibraryId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("stationerySpot.Models.OrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__OrderDet__3214EC07FA907AEA");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("stationerySpot.Models.PrintRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Copies")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("DocumentPath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("LibraryId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("PrintSide")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasDefaultValue("Pending");

                    b.HasKey("Id")
                        .HasName("PK__PrintReq__3214EC073D1603F7");

                    b.HasIndex("CustomerId");

                    b.HasIndex("LibraryId");

                    b.ToTable("PrintRequests");
                });

            modelBuilder.Entity("stationerySpot.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImagePath")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasDefaultValueSql("(NULL)");

                    b.Property<int>("LibraryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Products__3214EC07E57F5FF0");

                    b.HasIndex("CategoryId");

                    b.HasIndex("LibraryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("stationerySpot.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("LibraryId")
                        .HasColumnType("int");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Reviews__3214EC072F847FB9");

                    b.HasIndex("LibraryId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("stationerySpot.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("LibraryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("Id")
                        .HasName("PK__Services__3214EC073232CFBF");

                    b.HasIndex("LibraryId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("stationerySpot.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Users__3214EC07230112E2");

                    b.HasIndex(new[] { "Email" }, "UQ__Users__A9D105347FFC41AB")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("stationerySpot.Models.ContactU", b =>
                {
                    b.HasOne("stationerySpot.Models.Library", null)
                        .WithMany("ContactUs")
                        .HasForeignKey("LibraryId");

                    b.HasOne("stationerySpot.Models.Library", "Receiver")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ContactUs__Recei__17F790F9");

                    b.HasOne("stationerySpot.Models.User", "Sender")
                        .WithMany("ContactUs")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK__ContactUs__Sende__17036CC0");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("stationerySpot.Models.Library", b =>
                {
                    b.HasOne("stationerySpot.Models.User", "Owner")
                        .WithMany("Libraries")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("stationerySpot.Models.LibraryAccount", b =>
                {
                    b.HasOne("stationerySpot.Models.Library", "Library")
                        .WithMany("LibraryAccounts")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_LibraryAccounts_Libraries");

                    b.Navigation("Library");
                });

            modelBuilder.Entity("stationerySpot.Models.Order", b =>
                {
                    b.HasOne("stationerySpot.Models.User", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK__Orders__Customer__4D94879B");

                    b.HasOne("stationerySpot.Models.Library", "Library")
                        .WithMany("Orders")
                        .HasForeignKey("LibraryId")
                        .IsRequired()
                        .HasConstraintName("FK__Orders__LibraryI__4E88ABD4");

                    b.Navigation("Customer");

                    b.Navigation("Library");
                });

            modelBuilder.Entity("stationerySpot.Models.OrderDetail", b =>
                {
                    b.HasOne("stationerySpot.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__OrderDeta__Order__52593CB8");

                    b.HasOne("stationerySpot.Models.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .IsRequired()
                        .HasConstraintName("FK_OrderDetails_Products");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("stationerySpot.Models.PrintRequest", b =>
                {
                    b.HasOne("stationerySpot.Models.User", "Customer")
                        .WithMany("PrintRequests")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK__PrintRequ__Custo__0B91BA14");

                    b.HasOne("stationerySpot.Models.Library", "Library")
                        .WithMany("PrintRequests")
                        .HasForeignKey("LibraryId")
                        .IsRequired()
                        .HasConstraintName("FK__PrintRequ__Libra__0C85DE4D");

                    b.Navigation("Customer");

                    b.Navigation("Library");
                });

            modelBuilder.Entity("stationerySpot.Models.Product", b =>
                {
                    b.HasOne("stationerySpot.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK__Products__Catego__3E1D39E1");

                    b.HasOne("stationerySpot.Models.Library", "Library")
                        .WithMany("Products")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Products__Librar__4316F928");

                    b.Navigation("Category");

                    b.Navigation("Library");
                });

            modelBuilder.Entity("stationerySpot.Models.Review", b =>
                {
                    b.HasOne("stationerySpot.Models.Library", "Library")
                        .WithMany("Reviews")
                        .HasForeignKey("LibraryId")
                        .IsRequired()
                        .HasConstraintName("FK__Reviews__Library__6A30C649");

                    b.HasOne("stationerySpot.Models.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Reviews__UserId__693CA210");

                    b.Navigation("Library");

                    b.Navigation("User");
                });

            modelBuilder.Entity("stationerySpot.Models.Service", b =>
                {
                    b.HasOne("stationerySpot.Models.Library", "Library")
                        .WithMany("Services")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Services__Librar__208CD6FA");

                    b.Navigation("Library");
                });

            modelBuilder.Entity("stationerySpot.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("stationerySpot.Models.Library", b =>
                {
                    b.Navigation("ContactUs");

                    b.Navigation("LibraryAccounts");

                    b.Navigation("Orders");

                    b.Navigation("PrintRequests");

                    b.Navigation("Products");

                    b.Navigation("ReceivedMessages");

                    b.Navigation("Reviews");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("stationerySpot.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("stationerySpot.Models.Product", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("stationerySpot.Models.User", b =>
                {
                    b.Navigation("ContactUs");

                    b.Navigation("Libraries");

                    b.Navigation("Orders");

                    b.Navigation("PrintRequests");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
