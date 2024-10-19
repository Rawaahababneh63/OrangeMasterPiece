using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Advertise> Advertises { get; set; }

    public virtual DbSet<BeneficiaryTransaction> BeneficiaryTransactions { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Negotiation> Negotiations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<SaleRequest> SaleRequests { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ORANGE;Database=ProjectMasterPiece;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__4A311D2FEBAEB57C");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("Admin_id");
            entity.Property(e => e.Email).IsUnicode(false);
        });

        modelBuilder.Entity<Advertise>(entity =>
        {
            entity.HasKey(e => e.AdvertiseId).HasName("PK__Advertis__2881B9DA19B392FD");

            entity.ToTable("Advertise");

            entity.Property(e => e.AdvertiseId).HasColumnName("Advertise_id");
            entity.Property(e => e.AdTitle).HasMaxLength(200);
            entity.Property(e => e.AdvertiserEmail).HasMaxLength(100);
            entity.Property(e => e.AdvertiserName).HasMaxLength(100);
            entity.Property(e => e.Budget).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<BeneficiaryTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Benefici__55433A6B6B0B70DD");

            entity.Property(e => e.SaleAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Organization).WithMany(p => p.BeneficiaryTransactions)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Beneficia__Organ__7F2BE32F");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__D6862FC1393993BF");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.UserId, "UQ__Cart__206A9DF987B4A5CC").IsUnique();

            entity.Property(e => e.CartId).HasColumnName("Cart_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.UserId)
                .HasConstraintName("FK__Cart__User_id__5BE2A6F2");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__Cart_Ite__1FCDFEBC9E669F09");

            entity.ToTable("Cart_Item");

            entity.Property(e => e.CartItemId).HasColumnName("Cart_item_id");
            entity.Property(e => e.CartId).HasColumnName("Cart_id");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Cart_Item__Cart___5FB337D6");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Cart_Item__Produ__5EBF139D");

            entity.HasOne(d => d.User).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Cart_Item__User___60A75C0F");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__6DB2813615373072");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("Category_id");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChatMess__3214EC07A7A23A0C");

            entity.Property(e => e.Recipient).HasMaxLength(100);
            entity.Property(e => e.Sender).HasMaxLength(100);
            entity.Property(e => e.SentAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Colors__795A112C3739DB24");

            entity.Property(e => e.ColorId).HasColumnName("Color_id");
            entity.Property(e => e.ColorName).HasMaxLength(50);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__99D3E6C384A76E17");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("Comment_id");
            entity.Property(e => e.Comment1).HasColumnName("Comment");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Comment__Product__6EF57B66");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Comment__User_id__6E01572D");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__024E7A86304D8ACE");

            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.AdminResponse).HasColumnName("admin_response");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.ResponseDate).HasColumnName("response_date");
            entity.Property(e => e.SentDate).HasColumnName("sent_date");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .HasColumnName("subject");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC076ED31BBE");

            entity.Property(e => e.Timestamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<Negotiation>(entity =>
        {
            entity.HasKey(e => e.NegotiationId).HasName("PK__Negotiat__6D5328CEB5AC711C");

            entity.ToTable("Negotiation");

            entity.Property(e => e.NegotiationId).HasColumnName("Negotiation_id");
            entity.Property(e => e.FinalOffer).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.InitialOffer).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Negotiations)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Negotiati__Produ__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.Negotiations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Negotiati__User___74AE54BC");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__46596229191204C2");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VoucherId).HasColumnName("Voucher_id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Orders__user_id__66603565");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Orders__Voucher___6754599E");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__Order_It__483A64F9D82F7498");

            entity.ToTable("Order_Item");

            entity.Property(e => e.OrderItemId).HasColumnName("Order_Item_id");
            entity.Property(e => e.OrderId).HasColumnName("Order_id");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Order_Ite__Order__6A30C649");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Order_Ite__Produ__6B24EA82");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId).HasName("PK__Organiza__CADB0B1267074E52");

            entity.Property(e => e.BankAccount).HasMaxLength(100);
            entity.Property(e => e.ContactInfo).HasMaxLength(255);
            entity.Property(e => e.OrganizationName).HasMaxLength(100);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A388064F10B");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(100);
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.TransactionId).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Payments__UserId__7A672E12");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__9833FF92C2AB687A");

            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.ClothColorId).HasColumnName("Cloth_color_id");
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Condition).HasMaxLength(100);
            entity.Property(e => e.IsDonation).HasDefaultValue(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PriceWithDiscount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Price_with_discount");
            entity.Property(e => e.TypeProduct).HasMaxLength(100);

            entity.HasOne(d => d.ClothColor).WithMany(p => p.Products)
                .HasForeignKey(d => d.ClothColorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Products__Cloth___4E88ABD4");

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubcategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Products__Subcat__4D94879B");
        });

        modelBuilder.Entity<SaleRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__SaleRequ__33A8517A1D60E518");

            entity.Property(e => e.RequestDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RequestedPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Admin).WithMany(p => p.SaleRequests)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SaleReque__Admin__5535A963");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleRequests)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SaleReque__Produ__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.SaleRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SaleReque__UserI__534D60F1");
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.SubcategoryId).HasName("PK__Subcateg__9C4E705D7B947389");

            entity.ToTable("Subcategory");

            entity.Property(e => e.CategoryId).HasColumnName("Category_id");
            entity.Property(e => e.SubcategoryName).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Subcategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Subcatego__Categ__3B75D760");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206A9DF8FDBED870");

            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.PhoneNumber)
                .IsUnicode(false)
                .HasColumnName("Phone_number");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Vouchers__D753917C7BEF51CA");

            entity.Property(e => e.VoucherId).HasColumnName("Voucher_Id");
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
