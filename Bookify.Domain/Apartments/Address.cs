namespace Bookify.Domain.Apartments;

/*
 1.uniquely identified by its values
 2.immutability
 */

/// <summary>
/// value object represents address of apartment
/// </summary>
public record Address(
         string Country,
         string State,
         string ZipCode,
         string City,
         string Street
);

/*
 1.uniquely identified by its values
 2.immutability
 */