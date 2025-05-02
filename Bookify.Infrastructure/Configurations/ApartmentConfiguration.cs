using Bookify.Domain.Apartments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Bookify.Infrastructure.Configurations;

internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.ToTable("Apartments");

        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Address);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .HasConversion(name => name.value, value => new Name(value));

        builder.Property(x => x.Description)
            .HasMaxLength(2000)
            .HasConversion(descripion => descripion.value, value => new Description(value));

        builder.Property(x => x.Amenities)
            .HasConversion(
            x => JsonSerializer.Serialize(x, (JsonSerializerOptions)null),
            x => string.IsNullOrWhiteSpace(x) ? new List<Amenity>() : JsonSerializer.Deserialize<List<Amenity>>(x, (JsonSerializerOptions)null));

        builder.OwnsOne(x => x.Price, priceBuilder =>
        {
            priceBuilder.Property(mony => mony.Currency)
                .HasConversion(currency => currency.Code, code => Domain.Shared.Currency.FromCode(code));
        });

        builder.OwnsOne(x => x.CleaningFee, cleaningFeeBuilder =>
        {
            cleaningFeeBuilder.Property(x => x.Currency)
                .HasConversion(x => x.Code, code => Domain.Shared.Currency.FromCode(code));
        });

        // shadow property 
        builder.Property<byte[]>("Version").IsRowVersion().IsConcurrencyToken(); ;
    }
}
