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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Properties Configuration

            builder.ApplyConfiguration(new BrandConfiguration());
            builder.ApplyConfiguration(new MainCategoryConfiguration());
            builder.ApplyConfiguration(new SubCategoryConfiguration());
            builder.ApplyConfiguration(new UserAddressConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
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
