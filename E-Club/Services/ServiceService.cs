namespace E_Club.Services;

public class ServiceService : IServiceService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(
        ApplicationDbContext context,
        ILogger<ServiceService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<QuickServicesResponse>> GetQuickServicesAsync()
    {
        try
        {
            var services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .Take(3) // أول 3 خدمات
                .ToListAsync();

            var response = new QuickServicesResponse(
                Services: services.Select(MapToResponse).ToList()
            );

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting quick services");
            return Result.Failure<QuickServicesResponse>(
                new Error("Service.Error", "An error occurred while fetching services", 500)
            );
        }
    }

    public async Task<Result<IEnumerable<ServiceResponse>>> GetAllServicesAsync()
    {
        try
        {
            var services = await _context.Services
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();

            return Result.Success(services.Select(MapToResponse));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all services");
            return Result.Failure<IEnumerable<ServiceResponse>>(
                new Error("Service.Error", "An error occurred while fetching services", 500)
            );
        }
    }

    public async Task<Result<ServiceResponse>> GetServiceByIdAsync(int id)
    {
        try
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return Result.Failure<ServiceResponse>(
                    new Error("Service.NotFound", "Service not found", 404)
                );

            return Result.Success(MapToResponse(service));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service {ServiceId}", id);
            return Result.Failure<ServiceResponse>(
                new Error("Service.Error", "An error occurred while fetching the service", 500)
            );
        }
    }

    public async Task<Result<ServiceResponse>> CreateServiceAsync(CreateServiceRequest request, string userId)
    {
        try
        {
            var service = new Service
            {
                Name = request.Name,
                Description = request.Description,
                Icon = request.Icon,
                Endpoint = request.Endpoint,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive,
                DisplayOrder = request.DisplayOrder,
                Type = Enum.Parse<ServiceType>(request.Type),
                CreatedById = userId,
                CreatedOn = DateTime.UtcNow
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return Result.Success(MapToResponse(service));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service");
            return Result.Failure<ServiceResponse>(
                new Error("Service.Error", "An error occurred while creating the service", 500)
            );
        }
    }

    public async Task<Result> UpdateServiceAsync(int id, CreateServiceRequest request)
    {
        try
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return Result.Failure(
                    new Error("Service.NotFound", "Service not found", 404)
                );

            service.Name = request.Name;
            service.Description = request.Description;
            service.Icon = request.Icon;
            service.Endpoint = request.Endpoint;
            service.ImageUrl = request.ImageUrl;
            service.IsActive = request.IsActive;
            service.DisplayOrder = request.DisplayOrder;
            service.Type = Enum.Parse<ServiceType>(request.Type);
            service.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service {ServiceId}", id);
            return Result.Failure(
                new Error("Service.Error", "An error occurred while updating the service", 500)
            );
        }
    }

    public async Task<Result> DeleteServiceAsync(int id)
    {
        try
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return Result.Failure(
                    new Error("Service.NotFound", "Service not found", 404)
                );

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service {ServiceId}", id);
            return Result.Failure(
                new Error("Service.Error", "An error occurred while deleting the service", 500)
            );
        }
    }

    public async Task<Result> ToggleServiceStatusAsync(int id)
    {
        try
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return Result.Failure(
                    new Error("Service.NotFound", "Service not found", 404)
                );

            service.IsActive = !service.IsActive;
            service.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling service status {ServiceId}", id);
            return Result.Failure(
                new Error("Service.Error", "An error occurred while toggling service status", 500)
            );
        }
    }

    private ServiceResponse MapToResponse(Service service)
    {
        return new ServiceResponse(
            Id: service.Id,
            Name: service.Name,
            Description: service.Description,
            Icon: service.Icon,
            Endpoint: service.Endpoint,
            ImageUrl: service.ImageUrl,
            IsActive: service.IsActive,
            DisplayOrder: service.DisplayOrder,
            Type: service.Type.ToString()
        );
    }
}