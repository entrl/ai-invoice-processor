using InvoiceProcessor.Application.Common;
using InvoiceProcessor.Domain.Categories;

namespace InvoiceProcessor.Application.Categories;

public class CategoryService(ICategoryRepository repository) : ICategoryService
{
    public async Task<List<CategoryResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await repository.GetAllAsync(ct);
        return categories.Select(ToResponse).ToList();
    }

    public async Task<CategoryResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var category = await repository.GetByIdAsync(id, ct);
        return category is null ? null : ToResponse(category);
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default)
    {
        if (await repository.ExistsWithNameAsync(request.Name, ct))
            throw new ConflictException($"A category named '{request.Name}' already exists.");

        var category = new Category(request.Name);
        await repository.AddAsync(category, ct);
        await repository.SaveChangesAsync(ct);
        return ToResponse(category);
    }

    public async Task<CategoryResponse> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken ct = default)
    {
        var category = await repository.GetByIdAsync(id, ct)
                       ?? throw new NotFoundException($"Category {id} was not found.");

        if (category.Name != request.Name && await repository.ExistsWithNameAsync(request.Name, ct))
            throw new ConflictException($"A category named '{request.Name}' already exists.");

        category.Rename(request.Name);
        await repository.SaveChangesAsync(ct);
        return ToResponse(category);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var category = await repository.GetByIdAsync(id, ct)
                       ?? throw new NotFoundException($"Category {id} was not found.");

        await repository.DeleteAsync(category, ct);
        await repository.SaveChangesAsync(ct);
    }
    

    private static CategoryResponse ToResponse(Category c) => new() { Id = c.Id, Name = c.Name };
}