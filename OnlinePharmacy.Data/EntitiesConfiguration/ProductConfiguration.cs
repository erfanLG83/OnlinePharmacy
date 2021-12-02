using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlinePharmacy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Data.EntitiesConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.Property(x => x.Available)
                .HasDefaultValue(true);
            builder.Property(x => x.Barcode)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.CompoundsList)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(x => x.ConsumptionInstruction)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(x => x.EnglishName)
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(x => x.HowMaintain)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(x => x.Info1)
                .HasMaxLength(20)
                .IsRequired(false);
            builder.Property(x => x.Info2)
                .HasMaxLength(100)
                .IsRequired(false);
            builder.Property(x => x.Model)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(x => x.Price)
                .IsRequired();
            builder.Property(x => x.PropertiesList)
                .IsRequired();
            builder.Property(x => x.RecommendationsAndWarningsList)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(x => x.StoreCode)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.UrlAddress)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
