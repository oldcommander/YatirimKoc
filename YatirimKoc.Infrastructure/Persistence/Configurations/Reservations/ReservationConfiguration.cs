using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YatirimKoc.Domain.Entities.Reservations;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Reservations
{
    public class ReservationConfiguration
        : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.TimeSlot)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Note)
                .HasColumnType("longtext");

            builder.HasOne(x => x.AdminProfile)
                .WithMany()
                .HasForeignKey(x => x.AdminProfileId);

            builder.HasIndex(x => x.ReservationDate);
            builder.HasIndex(x => x.Status);
        }
    }

}
