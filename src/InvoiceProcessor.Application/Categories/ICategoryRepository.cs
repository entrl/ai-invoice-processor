using InvoiceProcessor.Domain.Categories;

namespace InvoiceProcessor.Application.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Category>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Category category, CancellationToken ct = default);
    Task<bool> ExistsWithNameAsync(string name, CancellationToken ct = default);
    Task DeleteAsync(Category category, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}