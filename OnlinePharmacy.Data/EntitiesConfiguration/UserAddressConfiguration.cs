using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlinePharmacy.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Data.EntitiesConfiguration
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.Property(x => x.Address1)
                .HasMaxLength(500)
                .IsRequired();
            builder.Property(x => x.Address2)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(x => x.CityName)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.CompanyName)
                .HasMaxLength(150)
                .IsRequired(false);
            builder.Property(x => x.PostalCode)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.ProvinceName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
