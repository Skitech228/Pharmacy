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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId)
                    .HasColumnName("UserId")
                    .HasColumnType("INTEGER")
                    .HasMaxLength(100)
                    .IsRequired();

            builder.Property(x => x.Telephone)
                    .HasColumnName("Telephone")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(100)
                    .IsRequired();
        }
    }
}
