using Microsoft.AspNetCore.Http;

public interface IProductImportService
{
    Task<(bool Success, string? Error, int Count)>
        ImportAsync(IFormFile file);
}