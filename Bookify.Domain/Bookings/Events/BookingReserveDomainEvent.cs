using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingReserveDomainEvent(Guid BookId) : IDomainEvent;
