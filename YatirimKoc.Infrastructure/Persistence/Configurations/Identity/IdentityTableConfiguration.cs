using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Identity;

public class IdentityTableConfiguration :
    IEntityTypeConfiguration<User>,
    IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
    }

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
    }

}
