using InvoiceProcessor.Domain.Vendors;

namespace InvoiceProcessor.Application.Vendors;

public interface IVendorRepository
{
    Task<Vendor?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Vendor>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Vendor vendor, CancellationToken ct = default);
    Task<bool> ExistsWithNameAsync(string name, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}