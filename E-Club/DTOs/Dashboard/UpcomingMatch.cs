namespace E_Club.DTOs.Dashboard;

public record UpcomingMatch(
    string MatchId,
    string HomeTeam,
    string AwayTeam,
    DateTime MatchDate,
    string Stadium,
    int AvailableSeats
);