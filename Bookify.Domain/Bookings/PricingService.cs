using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public sealed class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartment, DateRange dateRange)
    {
        var priceForPeriod = apartment.Price * dateRange.LengthInDays;

        decimal percentageUpCharge = 0;

        foreach (var amenity in apartment.Amenities)
        {
            percentageUpCharge += amenity switch
            {
                Amenity.GardenView or Amenity.MountainView => 0.05m,
                Amenity.AirConditioning => 0.01m,
                Amenity.Parking => 0.01m,
                _ => 0
            };
        }

        var amenitiesCharge = Money.Zero();
        if (percentageUpCharge > 0)
        {
            amenitiesCharge = priceForPeriod * percentageUpCharge;
        }

        var totalPrice = Money.Zero();

        if (!apartment.CleaningFee.IsZero())
        {
            totalPrice += apartment.CleaningFee;
        }

        totalPrice += amenitiesCharge;

        return new PricingDetails(priceForPeriod, apartment.CleaningFee, amenitiesCharge, totalPrice);
    }
}
