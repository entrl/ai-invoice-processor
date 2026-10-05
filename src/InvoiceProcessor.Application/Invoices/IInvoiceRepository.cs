using InvoiceProcessor.Domain.Invoices;

namespace InvoiceProcessor.Application.Invoices;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Invoice>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Invoice invoice, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid vendorId, string invoiceNumber, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}