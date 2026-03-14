namespace E_Club.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(
            ApplicationDbContext context,
            ILogger<DashboardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<DashboardDataResponse>> GetDashboardDataAsync(string userId)
        {
            try
            {
                // جلب بيانات المستخدم
                var user = await _context.Users
                    .Include(u => u.RefreshTokens)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return Result.Failure<DashboardDataResponse>(UserErrors.UserNotFound);

                // جلب المباريات القادمة (مثال)
                var upcomingMatches = new List<UpcomingMatch>
                {
                    new UpcomingMatch(
                        MatchId: "1",
                        HomeTeam: "الأهلي",
                        AwayTeam: "الزمالك",
                        MatchDate: DateTime.UtcNow.AddDays(3),
                        Stadium: "استاد القاهرة",
                        AvailableSeats: 15000
                    ),
                    new UpcomingMatch(
                        MatchId: "2",
                        HomeTeam: "بيراميدز",
                        AwayTeam: "الاتحاد",
                        MatchDate: DateTime.UtcNow.AddDays(5),
                        Stadium: "استاد برج العرب",
                        AvailableSeats: 8000
                    )
                };

                // جلب الحجوزات الأخيرة (مثال)
                var recentBookings = new List<Booking>
                {
                    new Booking(
                        BookingId: "B001",
                        SportName: "كرة قدم",
                        BookingDate: DateTime.UtcNow.AddDays(-1),
                        Status: "مؤكد",
                        CoachName: "أحمد حسن"
                    ),
                    new Booking(
                        BookingId: "B002",
                        SportName: "سباحة",
                        BookingDate: DateTime.UtcNow.AddDays(-2),
                        Status: "مكتمل",
                        CoachName: null
                    )
                };

                var response = new DashboardDataResponse(
                    MembershipType: user.MembershipId?.StartsWith("MEM") == true ? "Premium Member" : "Basic Member",
                    ClubName: "Elite Sports Club",
                    DigitalAccessKey: int.TryParse(user.DigitalAccessKey, out var key) ? key : 9842,
                    UpcomingMatches: upcomingMatches,
                    RecentBookings: recentBookings
                );

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data for user {UserId}", userId);
                return Result.Failure<DashboardDataResponse>(new Error(
                    "Dashboard.Error",
                    "An error occurred while fetching dashboard data",
                    StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<Result<StadiumDataResponse>> GetStadiumDataAsync()
        {
            try
            {
                // مثال لبيانات الاستاد
                var response = new StadiumDataResponse(
                    StadiumName: "استاد القاهرة الدولي",
                    MatchStatus: "PRO LEAGUE STANDARD",
                    SyncPercentage: 75,
                    LastUpdated: DateTime.UtcNow
                );

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stadium data");
                return Result.Failure<StadiumDataResponse>(new Error(
                    "Dashboard.Error",
                    "An error occurred while fetching stadium data",
                    StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<Result<BookingStatsResponse>> GetBookingStatsAsync()
        {
            try
            {
                // مثال لإحصائيات الحجوزات
                var sportStats = new List<SportBookingStats>
                {
                    new SportBookingStats("كرة قدم", 45, 20),
                    new SportBookingStats("سباحة", 30, 15),
                    new SportBookingStats("تنس", 25, 10),
                    new SportBookingStats("جيم", 60, 40)
                };

                var response = new BookingStatsResponse(
                    TotalBookings: 160,
                    TodayBookings: 12,
                    UpcomingBookings: 48,
                    SportStats: sportStats
                );

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking stats");
                return Result.Failure<BookingStatsResponse>(new Error(
                    "Dashboard.Error",
                    "An error occurred while fetching booking statistics",
                    StatusCodes.Status500InternalServerError));
            }
        }
    }
}