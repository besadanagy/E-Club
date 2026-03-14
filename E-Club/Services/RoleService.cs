
namespace E_Club.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;  // محتاجين نضيفها

        public RoleService(
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)  // إضافة UserManager
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default)
        {
            var query = _roleManager.Roles.AsQueryable();

            // لو عايز تجيب الكل (من غير فلترة IsDefault)
            if (includeDisabled != true)
                query = query.Where(r => !r.IsDeleted);
            // متفلترش على IsDefault خالص!

            var roles = await query.ToListAsync(cancellationToken);
            return roles.Select(r => new RoleResponse(r.Id, r.Name!, r.IsDeleted));
        }

        public async Task<Result<RoleDetailsResponse>> GetAsync(string id)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleNotFound);

            // Get permissions from RoleClaims
            var permissions = await _context.RoleClaims
                .Where(x => x.RoleId == role.Id && x.ClaimType == Permissions.Type)
                .Select(x => x.ClaimValue!)
                .ToListAsync();

            // Get users in this role
            var usersInRole = new List<UserInRoleResponse>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var isInRole = await _userManager.IsInRoleAsync(user, role.Name!);
                if (isInRole)
                {
                    usersInRole.Add(new UserInRoleResponse(
                        user.Id,
                        user.UserName ?? user.Email!,
                        user.Email!,
                        true
                    ));
                }
            }

            var response = new RoleDetailsResponse(
                role.Id,
                role.Name!,
                role.IsDeleted,
                permissions,
                usersInRole
            );

            return Result.Success(response);
        }

        public async Task<Result<RoleDetailsResponse>> AddAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return Result.Failure<RoleDetailsResponse>(new Error("Role.InvalidName", "Role name is required", StatusCodes.Status400BadRequest));

            var roleExists = await _roleManager.RoleExistsAsync(request.Name);
            if (roleExists)
                return Result.Failure<RoleDetailsResponse>(new Error("Role.AlreadyExists", "Role already exists", StatusCodes.Status400BadRequest));

            var role = new ApplicationRole(request.Name);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure<RoleDetailsResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            // Add permissions to role
            var usersInRole = new List<UserInRoleResponse>();

            if (request.Permissions != null && request.Permissions.Any())
            {
                // Validate permissions
                var allowedPermissions = Permissions.GetAllPermissions();
                if (request.Permissions.Except(allowedPermissions).Any())
                    return Result.Failure<RoleDetailsResponse>(RoleErrors.InvalidPermission);

                foreach (var permission in request.Permissions)
                {
                    await _context.RoleClaims.AddAsync(new IdentityRoleClaim<string>
                    {
                        RoleId = role.Id,
                        ClaimType = Permissions.Type,
                        ClaimValue = permission
                    });
                }
                await _context.SaveChangesAsync();
            }

            // Add users to role if specified
            if (request.UsersIds != null && request.UsersIds.Any())
            {
                foreach (var userId in request.UsersIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name!);
                        usersInRole.Add(new UserInRoleResponse(
                            user.Id,
                            user.UserName ?? user.Email!,
                            user.Email!,
                            true
                        ));
                    }
                }
            }

            var response = new RoleDetailsResponse(
                role.Id,
                role.Name!,
                role.IsDeleted,
                request.Permissions ?? new List<string>(),
                usersInRole
            );

            return Result.Success(response);
        }


        public async Task<Result> ToggleStatusAsync(string id)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure(RoleErrors.RoleNotFound);

            if (role.IsDefault)
                return Result.Failure(RoleErrors.CannotDeleteDefaultRole);

            role.IsDeleted = !role.IsDeleted;
            await _roleManager.UpdateAsync(role);

            return Result.Success();
        }

        public async Task<Result> UpadteAsync(string id, RoleRequest request)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure(RoleErrors.RoleNotFound);

            // Check for duplicate role name
            var roleIsExist = await _roleManager.Roles
                .AnyAsync(x => x.Name == request.Name && x.Id != id);

            if (roleIsExist)
                return Result.Failure(RoleErrors.DuplicatedRole);

            // Validate permissions
            if (request.Permissions != null)
            {
                var allowedPermissions = Permissions.GetAllPermissions();
                if (request.Permissions.Except(allowedPermissions).Any())
                    return Result.Failure(RoleErrors.InvalidPermission);
            }

            // Update role name
            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            // Update permissions
            if (request.Permissions != null)
            {
                var currentPermissions = await _context.RoleClaims
                    .Where(x => x.RoleId == id && x.ClaimType == Permissions.Type)
                    .Select(x => x.ClaimValue)
                    .ToListAsync();

                var newPermissions = request.Permissions
                    .Except(currentPermissions)
                    .Select(p => new IdentityRoleClaim<string>
                    {
                        ClaimType = Permissions.Type,
                        ClaimValue = p,
                        RoleId = role.Id
                    });

                var removedPermissions = currentPermissions
                    .Except(request.Permissions)
                    .ToList();

                // Remove old permissions
                if (removedPermissions.Any())
                {
                    await _context.RoleClaims
                        .Where(x => x.RoleId == id &&
                                   removedPermissions.Contains(x.ClaimValue) &&
                                   x.ClaimType == Permissions.Type)
                        .ExecuteDeleteAsync();
                }

                // Add new permissions
                if (newPermissions.Any())
                {
                    await _context.AddRangeAsync(newPermissions);
                }

                await _context.SaveChangesAsync();
            }

            // Update users in role
            if (request.UsersIds != null)
            {
                var currentUsers = await _userManager.GetUsersInRoleAsync(role.Name!);

                // Remove users not in the new list
                foreach (var user in currentUsers)
                {
                    if (!request.UsersIds.Contains(user.Id))
                        await _userManager.RemoveFromRoleAsync(user, role.Name!);
                }

                // Add new users
                foreach (var userId in request.UsersIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null && !await _userManager.IsInRoleAsync(user, role.Name!))
                        await _userManager.AddToRoleAsync(user, role.Name!);
                }
            }

            return Result.Success();
        }
    }
}