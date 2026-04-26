namespace E_Club.Services;

public class BannerService(ApplicationDbContext context, ILogger<BannerService> logger) : IBannerService
{
    public async Task<Result<IEnumerable<BannerResponse>>> GetActiveBannersAsync()
    {
        var banners = await context.Banners
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .Select(b => new BannerResponse(b.Id, b.Title, b.Subtitle, b.ImageUrl, b.ActionUrl, b.Type.ToString()))
            .ToListAsync();

        return Result.Success(banners.AsEnumerable());
    }

    public async Task<Result<BannerResponse>> GetByIdAsync(int id)
    {
        var banner = await context.Banners.FindAsync(id);

        return banner is null
            ? Result.Failure<BannerResponse>(BannerErrors.NotFound)
            : Result.Success(Map(banner));
    }

    public async Task<Result<BannerResponse>> CreateAsync(CreateBannerRequest request, string userId)
    {
        if (!Enum.TryParse<BannerType>(request.Type, out var type))
            return Result.Failure<BannerResponse>(BannerErrors.InvalidType);

        var banner = new Banner
        {
            Title = request.Title,
            Subtitle = request.Subtitle,
            ImageUrl = request.ImageUrl,
            ActionUrl = request.ActionUrl,
            IsActive = request.IsActive,
            DisplayOrder = request.DisplayOrder,
            Type = type,
            CreatedById = userId,
            CreatedOn = DateTime.UtcNow
        };

        context.Banners.Add(banner);
        await context.SaveChangesAsync();

        return Result.Success(Map(banner));
    }

    public async Task<Result> UpdateAsync(int id, CreateBannerRequest request)
    {
        var banner = await context.Banners.FindAsync(id);
        if (banner is null) return Result.Failure(BannerErrors.NotFound);

        if (!Enum.TryParse<BannerType>(request.Type, out var type))
            return Result.Failure(BannerErrors.InvalidType);

        banner.Title = request.Title;
        banner.Subtitle = request.Subtitle;
        banner.ImageUrl = request.ImageUrl;
        banner.ActionUrl = request.ActionUrl;
        banner.IsActive = request.IsActive;
        banner.DisplayOrder = request.DisplayOrder;
        banner.Type = type;
        banner.UpdatedOn = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var banner = await context.Banners.FindAsync(id);
        if (banner is null) return Result.Failure(BannerErrors.NotFound);

        context.Banners.Remove(banner);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(int id)
    {
        var banner = await context.Banners.FindAsync(id);
        if (banner is null) return Result.Failure(BannerErrors.NotFound);

        banner.IsActive = !banner.IsActive;
        banner.UpdatedOn = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success();
    }

    private static BannerResponse Map(Banner b) =>
        new(b.Id, b.Title, b.Subtitle, b.ImageUrl, b.ActionUrl, b.Type.ToString());
}

public static class BannerErrors
{
    public static readonly Error NotFound = new("Banner.NotFound", "Banner not found", 404);
    public static readonly Error InvalidType = new("Banner.InvalidType", "Invalid banner type", 400);
}
