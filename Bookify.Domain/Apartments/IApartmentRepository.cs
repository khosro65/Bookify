namespace Bookify.Domain.Apartments;

public interface IApartmentRepository
{
    Task<Apartment?> GEtByIdAsync(CancellationToken cancellationToken = default);
}
