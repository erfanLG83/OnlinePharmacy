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
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.ToTable("SubCategories");
            builder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(x => x.EnglishName)
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(x => x.ImageName)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(x => x.IntroductionImageName)
                .HasMaxLength(500)
                .IsRequired(true);
            builder.Property(x => x.IntroductionText)
                .IsRequired(true);
        }
    }
}
