using Bookify.Domain.Abstraction;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Apartments;

public sealed class Apartment(
    Guid id,
    Name name,
    Description description,
    Address address,
    Money price,
    Money cleaningFee,
    List<Amenity> amenities) : Entity(id)
{
    public Name Name { get; private set; } = name;
    public Description Description { get; private set; } = description;
    public Address Address { get; private set; } = address;
    public Money Price { get; private set; } = price;
    public Money CleaningFee { get; private set; } = cleaningFee;
    public DateTime? LastBookedOnUtc { get; private set; }
    public List<Amenity> Amenities { get; private set; } = amenities;
}
/*
 why private setters?
    we don't want to allow any values of our entity
    to be able to change from outside of entity 
    if it was public somehow may value changed outside and 
    break the invariance that is our defensive design

In Domain-Driven Design (DDD), using private setters in entity models is a strategic choice that helps preserve the integrity and consistency of the domain model. Here’s why:

🔒 1. Encapsulation of Invariants
Private setters prevent external code from modifying the internal state of an entity arbitrarily. This ensures that business rules and invariants are enforced through controlled methods (often called "behavior methods").

For example, if an Order should only change status via an Approve() method that checks if payment was successful, using a private setter on Status ensures that logic can't be bypassed.

✅ 2. Controlled Mutability
Entities can still change over time (they’re not immutable), but only in well-defined, intentional ways via methods inside the aggregate root. This ensures all changes are meaningful in the context of the domain.

🧩 3. Aggregate Root Integrity
When you use aggregates, only the root is responsible for coordinating changes. Private setters ensure that child entities or external consumers can’t break this contract by directly modifying internal state.

📦 4. Improved Testability and Maintenance
By exposing only behaviors and not raw properties, you make your code easier to refactor and test, because all changes to the entity state flow through a limited number of paths.
 */