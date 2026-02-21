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
    public class AdminAvailabilityConfiguration
        : IEntityTypeConfiguration<AdminAvailability>
    {
        public void Configure(EntityTypeBuilder<AdminAvailability> builder)
        {
            builder.ToTable("AdminAvailabilities");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SlotDurationMinutes)
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.AdminProfileId,
                x.DayOfWeek
            });
        }
    }

}
