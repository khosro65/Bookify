using Bogus;
using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Apartments;
using Dapper;


namespace Bookify.Api.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<object> apartments = new();
        for (var i = 0; i < 100; i++)
        {
            apartments.Add(new
            {
                Id = Guid.NewGuid(),
                Name = faker.Company.CompanyName(),
                Description = "Amazing view",
                Country = faker.Address.Country(),
                State = faker.Address.State(),
                ZipCode = faker.Address.ZipCode(),
                City = faker.Address.City(),
                Street = faker.Address.StreetAddress(),
                PriceAmount = faker.Random.Decimal(50, 1000),
                PriceCurrency = "USD",
                CleaningFeeAmount = faker.Random.Decimal(25, 200),
                CleaningFeeCurrency = "USD",
                Amenities = System.Text.Json.JsonSerializer.Serialize(new List<Amenity> { Amenity.Parking, Amenity.MountainView }),
                LastBookedOn = new DateTime(1999, 1, 1)                
            });
        }

        const string sql = """
            INSERT INTO dbo.apartments
            (id, [name], [Description], address_country, address_state, Address_ZipCode, address_city, address_street, price_amount, price_currency, CleaningFee_Amount, cleaningfee_currency, amenities, lastbookedonutc)
            VALUES(@Id, @Name, @Description, @Country, @State, @ZipCode, @City, @Street, @PriceAmount, @PriceCurrency, @CleaningFeeAmount, @CleaningFeeCurrency, @Amenities, @LastBookedOn)
            """;

        connection.Execute(sql, apartments);
    }
}
