namespace E_Club.DTOs.Dashboard;

public record StadiumDataResponse(
    string StadiumName,
    string MatchStatus,
    int SyncPercentage,
    DateTime LastUpdated
);