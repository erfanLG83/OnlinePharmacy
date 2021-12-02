using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.Data.EntitiesConfiguration;
using OnlinePharmacy.Domain.Auth;
using OnlinePharmacy.Domain.Entities;

namespace OnlinePharmacy.Data
{
    public class OnlinePharmacyDbContext
        : IdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>, AppUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Properties Configuration

            builder.ApplyConfiguration(new BrandConfiguration());
            builder.ApplyConfiguration(new MainCategoryConfiguration());
            builder.ApplyConfiguration(new SubCategoryConfiguration());
            builder.ApplyConfiguration(new UserAddressConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());

            builder.Entity<Order>().HasKey(x => x.ShoppingCartId);
            builder.Entity<Order>().Property(x => x.PaymentAuthority)
                .HasMaxLength(100)
                .IsRequired(false);
            builder.Entity<ProductImage>()
                .Property(x => x.ImageName)
                .HasMaxLength(500)
                .IsRequired();
            builder.Entity<AppUser>()
                .Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();
            builder.Entity<AppUser>()
                .Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();
            builder.Entity<AppUserRole>()
                .HasKey(n => new { 
                    n.RoleId,
                    n.UserId
                });
            #endregion

            #region Realations Configuration
            builder.Entity<Brand>()
                .HasMany(x => x.Products)
                .WithOne(x => x.Brand)
                .HasForeignKey(x => x.BrandId);
            builder.Entity<FavoriteProduct>()
                .HasOne(x => x.Product)
                .WithMany(x => x.FavoriteProducts)
                .HasForeignKey(x => x.ProductId);
            builder.Entity<FavoriteProduct>()
                .HasOne(x => x.User)
                .WithMany(x => x.FavoriteProducts)
                .HasForeignKey(x => x.UserId);
            builder.Entity<MainCategory>()
                .HasMany(x => x.Products)
                .WithOne(x => x.MainCategory)
                .HasForeignKey(x => x.MainCategoryId);
            builder.Entity<SubCategory>()
                .HasOne(x => x.MainCategory)
                .WithMany(x => x.SubCategories)
                .HasForeignKey(x => x.MainCategoryId);
            builder.Entity<SubCategory>()
                .HasOne(x => x.ParentSubCategory)
                .WithMany(x => x.ChildSubCategories)
                .HasForeignKey(x => x.ParentId);
            builder.Entity<SubCategory>()
                .HasMany(x => x.Products)
                .WithOne(x => x.SubCategory)
                .HasForeignKey(x => x.SubCategoryId);
            builder.Entity<ProductImage>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId);
            builder.Entity<AppUserRole>()
                .HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);
            builder.Entity<AppUserRole>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId);
            builder.Entity<ShoppingCart>()
                .HasMany(x => x.CartItems)
                .WithOne(x => x.ShoppingCart)
                .HasForeignKey(x => x.ShoppingCartId);
            builder.Entity<ShoppingCart>()
                .HasOne(x => x.User)
                .WithMany(x => x.ShoppingCarts)
                .HasForeignKey(x => x.UserId);
            builder.Entity<ShoppingCart>()
                .HasOne(x => x.Order)
                .WithOne(x => x.ShoppingCart)
                .HasForeignKey<Order>(x=>x.ShoppingCartId);
            builder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId);
            builder.Entity<Order>()
                .HasOne(x => x.UserAddress)
                .WithMany(x=>x.Orders)
                .HasForeignKey(x=>x.UserAdressId);
            #endregion

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=OnlinePharmacyDb;Trusted_Connection=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
