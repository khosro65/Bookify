namespace Bookify.Domain.Apartments;

public interface IApartmentRepository
{
    Task<Apartment?> GetByIdAsync(Guid apratmentId,CancellationToken cancellationToken = default);
}
