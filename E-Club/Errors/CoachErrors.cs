namespace E_Club.Errors;

public static class CoachErrors
{
    public static readonly Error CoachNotFound =
        new("Coach.NotFound", "Coach not found", StatusCodes.Status404NotFound);

    public static readonly Error CoachAlreadyAssigned =
        new("Coach.AlreadyAssigned", "This coach is already assigned to the class", StatusCodes.Status409Conflict);
}
