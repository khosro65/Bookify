using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users;

public class User: Entity
{

    private User(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Email email
        ) 
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
    public FirstName FirstName{ get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }


    public User Create(FirstName firstName, LastName lastName, Email email)
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email); 

        return user;
    }
}


/* 
 why using private constructor?
    1. hiding the constructor 
    2. encapsulation
    3. introduce some side effects inside factory that don't naturally belong to constructor 

| Purpose | Benefit |
|--------|---------|
| Validate complex creation logic | Prevents invalid states |
| Hide construction details | Clean and safe creation |
| Enforce invariants | Business logic is centralized |
| Improve code readability | Expressive and intention-revealing methods |
| Support persistence mechanisms | Works with tools like EF Core |
1. Enforce Invariants at Creation Time
Some entities or value objects have complex creation rules (e.g. validations, business constraints). A factory method centralizes these checks so you don't accidentally create invalid objects.

🛑 Without a factory, someone could call the constructor with invalid data.

2. Provide Meaningful and Intuitive Object Creation
Factory methods can have descriptive names that explain intent better than overloaded constructors.

3. Hide Construction Details
If creating an entity involves multiple steps, helper methods, or default values, you can hide that complexity behind a factory.

This is especially useful if:

You set default states or IDs internally.

Construction depends on external services (which might be injected into a factory class, not the entity).

4. Ensure Immutability (for Value Objects)
For value objects, a factory ensures they are fully initialized and immutable after creation, without exposing mutable state or default constructors.

5. Support Persistence Frameworks Cleanly
In ORMs like Entity Framework, a private parameterless constructor is often needed for hydration. You can:

Use the private constructor for EF.

Use the factory for your domain logic.
 */