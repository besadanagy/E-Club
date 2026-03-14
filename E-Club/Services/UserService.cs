namespace E_Club.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly ApplicationDbContext _context;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IRoleService roleService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleService = roleService;
            _context = context;
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .Where(u => !u.IsDisabled)
                .ToListAsync(cancellationToken);

            var userResponses = new List<UserResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userResponses.Add(new UserResponse(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email!,
                    user.IsDisabled,
                    roles
                ));
            }

            return userResponses;
        }

        public async Task<Result<UserResponse>> GetAsync(string id)
        {
            if (await _userManager.FindByIdAsync(id) is not { } user)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            var userRoles = await _userManager.GetRolesAsync(user);

            var response = new UserResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email!,
                user.IsDisabled,
                userRoles
            );

            return Result.Success(response);
        }

        public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            // Check if email already exists
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
                return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

            // Validate roles
            var allowedRoles = await _roleService.GetAllAsync(cancellationToken: cancellationToken);
            if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
                return Result.Failure<UserResponse>(UserErrors.InvalidRole);

            // Create user
            var user = request.Adapt<ApplicationUser>();
            user.UserName = request.Email;
            user.CreatedOn = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                if (request.Roles.Any())
                {
                    await _userManager.AddToRolesAsync(user, request.Roles);
                }

                var response = new UserResponse(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email!,
                    user.IsDisabled,
                    request.Roles
                );

                return Result.Success(response);
            }

            var error = result.Errors.First();
            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken)
        {
            // Check if email already exists for another user
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id, cancellationToken))
                return Result.Failure(UserErrors.DuplicatedEmail);

            // Validate roles
            var allowedRoles = await _roleService.GetAllAsync(cancellationToken: cancellationToken);
            if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
                return Result.Failure(UserErrors.InvalidRole);

            // Find user
            if (await _userManager.FindByIdAsync(id) is not { } user)
                return Result.Failure(UserErrors.UserNotFound);

            // Update user properties
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Update roles
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Remove roles that are not in the new list
                var rolesToRemove = currentRoles.Except(request.Roles);
                if (rolesToRemove.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                }

                // Add new roles
                var rolesToAdd = request.Roles.Except(currentRoles);
                if (rolesToAdd.Any())
                {
                    await _userManager.AddToRolesAsync(user, rolesToAdd);
                }

                return Result.Success();
            }

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ToggleStatusAsync(string id)
        {
            if (await _userManager.FindByIdAsync(id) is not { } user)
                return Result.Failure(UserErrors.UserNotFound);

            user.IsDisabled = !user.IsDisabled;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }


       
        public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

            var roles = await _userManager.GetRolesAsync(user);

            var response = new UserProfileResponse(
                user.Email!,
                user.UserName!,
                user.FirstName,
                user.LastName
            );

            return Result.Success(response);
        }

        public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            // Check if email already exists for another user
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != userId))
                return Result.Failure(UserErrors.DuplicatedEmail);

            // Update user properties
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> UnLockAsync(string id)
        {

            if (await _userManager.FindByIdAsync(id) is not { } user)
                return Result.Failure(UserErrors.UserNotFound);

            var result = await _userManager.SetLockoutEndDateAsync(user, null);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
    }
}