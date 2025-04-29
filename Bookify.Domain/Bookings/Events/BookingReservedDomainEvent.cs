using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingReservedDomainEvent(Guid BookId) : IDomainEvent;
public record BookingConfirmedDomainEvent(Guid BookId) : IDomainEvent;
public record BookingRejectedDomainEvent(Guid BookId) : IDomainEvent;
public record BookingCompletedDomainEvent(Guid BookId) : IDomainEvent;
public record BookingCancelledDomainEvent(Guid BookId) : IDomainEvent;