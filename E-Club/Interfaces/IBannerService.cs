namespace E_Club.Interfaces;

public interface IBannerService
{
    Task<Result<IEnumerable<BannerResponse>>> GetActiveBannersAsync();
    Task<Result<BannerResponse>> GetByIdAsync(int id);
    Task<Result<BannerResponse>> CreateAsync(CreateBannerRequest request, string userId);
    Task<Result> UpdateAsync(int id, CreateBannerRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> ToggleStatusAsync(int id);
}
