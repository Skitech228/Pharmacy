using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Domain.Entity;

namespace Pharmacy.Database.Conigurations
{
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.AddressId);

            builder.Property(x => x.City)
                    .HasColumnName("City")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(100)
                    .IsRequired();

            builder.Property(x => x.Street)
                    .HasColumnName("Street")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(100)
                    .IsRequired();
        }
    }
}
