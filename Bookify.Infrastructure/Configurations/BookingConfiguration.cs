using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.OwnsOne(x => x.PriceForPeriod, priceBuilder =>
        {
            priceBuilder.Property(x => x.Currency)
                .HasConversion(x => x.Code , code => Currency.FromCode(code));
        });



        builder.OwnsOne(x => x.CleaningFee, priceBuilder =>
        {
            priceBuilder.Property(x => x.Currency)
                .HasConversion(x => x.Code, code => Currency.FromCode(code));
        });



        builder.OwnsOne(x => x.TotalPrice, priceBuilder =>
        {
            priceBuilder.Property(x => x.Currency)
                .HasConversion(x => x.Code, code => Currency.FromCode(code));
        });


        builder.OwnsOne(x => x.AmenitiesUpCharge, priceBuilder =>
        {
            priceBuilder.Property(x => x.Currency)
                .HasConversion(x => x.Code, code => Currency.FromCode(code));
        });


        builder.HasOne<Apartment>()
            .WithMany()
            .HasForeignKey(x => x.ApartmentId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}
