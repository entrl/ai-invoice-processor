using InvoiceProcessor.Application.Categories;
using InvoiceProcessor.Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessor.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    
    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Categories.OrderBy(c => c.Name).ToListAsync(ct);
    }

    public async Task AddAsync(Category category, CancellationToken ct = default)
    {
        await _context.Categories.AddAsync(category, ct);
    }

    public Task<bool> ExistsWithNameAsync(string name, CancellationToken ct = default)
    {
        return _context.Categories.AnyAsync(c => c.Name == name, ct);
    }

    public Task DeleteAsync(Category category, CancellationToken ct = default)
    {
        _context.Categories.Remove(category);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}