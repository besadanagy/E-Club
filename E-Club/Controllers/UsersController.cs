namespace E_Club.Controllers
{
    [Route("api/users")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _userService.GetAsync(id);
            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> Add(
            [FromBody] CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _userService.AddAsync(request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            string id,
            [FromBody] UpdateUserRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var result = await _userService.ToggleStatusAsync(id);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpPut("{id}/unlock")]
        public async Task<IActionResult> Unlock(string id)
        {
            var result = await _userService.UnLockAsync(id);  // UnLockAsync → UnlockAsync

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
    }
}