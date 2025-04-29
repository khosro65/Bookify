using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings;

public static class BookingErrors
{
    public static Error NotFound = new(
        "Booking.NotFound",
        "Booking not found"
        );

    public static Error Overlap = new(
        "Booking.Overlap",
        "Booking overlaps with another booking"
        );

    public static Error NotReserved = new(
        "Booking.NotReserved",
        "The booking is not pending"
        );

    public static Error NotConfirmed = new(
        "Booking.NotConfirmed",
        "The booking is not confirmed"
        );

    public static Error AlreadyStarted = new (
        "Booking.AlreadyStarted",
        "The booking has already started"
        );
}
