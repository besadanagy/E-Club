
namespace E_Club.Interfaces;

public interface IDashboardService
{
    Task<Result<DashboardDataResponse>> GetDashboardDataAsync(string userId);
    Task<Result<StadiumDataResponse>> GetStadiumDataAsync();
    Task<Result<BookingStatsResponse>> GetBookingStatsAsync();
}