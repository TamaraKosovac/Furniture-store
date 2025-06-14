using Microsoft.EntityFrameworkCore;
using CozyHaven.Models;
using Microsoft.Extensions.Configuration;

namespace CozyHaven.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<CashRegister> CashRegisters { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<Bill> Bills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 36))
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.EmploymentDate)
                .HasColumnType("date");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<User>()
                .Property(u => u.Salary)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Department>()
                .Property(d => d.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Department>()
                .Property(d => d.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<CashRegister>()
                .Property(c => c.Number)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.SupplierPrice)
                .HasColumnType("decimal(10,2)");


            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BillItem>()
                .Property(b => b.UnitPrice)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<BillItem>()
                .HasOne(b => b.Product)
                .WithMany()
                .HasForeignKey(b => b.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
                .HasMany(b => b.Items)
                .WithOne(bi => bi.Bill)
                .HasForeignKey(bi => bi.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bill>()
                .Property(b => b.TotalAmount)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Bill>()
                .Property(b => b.PaymentMethod)
                .HasConversion<string>(); 
        }
    }
}
