
namespace E_Club.Interfaces;

public interface IServiceService
{
    // للكل
    Task<Result<QuickServicesResponse>> GetQuickServicesAsync();
    Task<Result<IEnumerable<ServiceResponse>>> GetAllServicesAsync();
    Task<Result<ServiceResponse>> GetServiceByIdAsync(int id);

    // للإدمن فقط
    Task<Result<ServiceResponse>> CreateServiceAsync(CreateServiceRequest request, string userId);
    Task<Result> UpdateServiceAsync(int id, CreateServiceRequest request);
    Task<Result> DeleteServiceAsync(int id);
    Task<Result> ToggleServiceStatusAsync(int id);
}