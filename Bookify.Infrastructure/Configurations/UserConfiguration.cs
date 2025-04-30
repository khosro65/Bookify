using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(t => t.Id);

        builder.Property(p => p.FirstName)
            .HasMaxLength(200)
            .HasConversion(x => x.value, x => new FirstName(x));


        builder.Property(p => p.LastName)
            .HasMaxLength(200)
            .HasConversion(x => x.value, x => new LastName(x));


        builder.Property(p => p.Email)
            .HasMaxLength(400)
            .HasConversion(x => x.value, x => new Domain.Users.Email(x));

        builder.HasIndex(x => x.Email).IsUnique();
    }
}
