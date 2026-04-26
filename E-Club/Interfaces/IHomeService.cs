namespace E_Club.Interfaces;

public interface IHomeService
{
    Task<Result<HomeResponse>> GetHomeDataAsync(string? userId = null);
}
