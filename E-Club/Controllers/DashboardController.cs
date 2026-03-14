
namespace E_Club.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IDashboardService dashboardService,
            ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _dashboardService.GetDashboardDataAsync(userId);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpGet("stadium")]
        public async Task<IActionResult> GetStadiumData()
        {
            var result = await _dashboardService.GetStadiumDataAsync();

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookingStats()
        {
            var result = await _dashboardService.GetBookingStatsAsync();

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }
    }
}