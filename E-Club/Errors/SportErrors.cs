namespace E_Club.Errors
{
    public static class SportErrors
    {
        public static readonly Error SportNotFound = new("Sport.NotFound", "Sport not found", 404);
        public static readonly Error ClassNotFound = new("Class.NotFound", "Class not found", 404);
        public static readonly Error ClassClosed = new("Class.Closed", "Class is no longer accepting bookings", 400);
        public static readonly Error ClassFull = new("Class.Full", "Class is fully booked", 400);
        public static readonly Error AlreadyBooked = new("Class.AlreadyBooked", "Already booked this class", 409);
        public static readonly Error BookingNotFound = new("Booking.NotFound", "Booking not found", 404);
        public static readonly Error AlreadyCancelled = new("Booking.AlreadyCancelled", "Booking already cancelled", 400);
        public static readonly Error NoSpecialEvent = new("Class.NoSpecialEvent", "No special event found", 404);
        public static readonly Error InvalidType = new("Class.InvalidType", "Invalid class type", 400);
    }

}
