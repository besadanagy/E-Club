namespace E_Club.Controllers
{
    [Route("api/roles")]
    [ApiController]
    //[Authorize(Roles = "Admin")]  // Uncomment when ready
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
            [FromQuery] bool? includeDisabled,
            CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAllAsync(includeDisabled, cancellationToken);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await _roleService.GetAsync(id);
            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] RoleRequest request)
        {
            var result = await _roleService.AddAsync(request);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetByIdAsync), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] RoleRequest request)
        {
            var result = await _roleService.UpadteAsync(id, request);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatusAsync(string id)
        {
            var result = await _roleService.ToggleStatusAsync(id);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
    }
}