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
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");
            builder.Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(x => x.EnglishName)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(x => x.IntroductionText)
                .IsRequired();
            builder.Property(x => x.ImageName)
                .HasMaxLength(500)
                .IsRequired(false);
        }
    }
}
