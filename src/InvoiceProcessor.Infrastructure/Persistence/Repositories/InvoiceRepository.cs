using InvoiceProcessor.Application.Invoices;
using InvoiceProcessor.Domain.Invoices;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessor.Infrastructure.Persistence.Repositories;

public class InvoiceRepository(AppDbContext context) : IInvoiceRepository
{
    public async Task<Invoice?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Invoices
            .Include(i => i.Vendor)
            .Include(i => i.LineItems)
            .FirstOrDefaultAsync(i => i.Id == id, ct);
    }

    public async Task<List<Invoice>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Invoices
            .AsNoTracking()
            .Include(i => i.Vendor)
            .Include(i => i.LineItems)
            .OrderByDescending(i => i.IssueDate)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Invoice invoice, CancellationToken ct = default)
    {
        await context.Invoices.AddAsync(invoice, ct);
    }

    public Task<bool> ExistsAsync(Guid vendorId, string invoiceNumber, CancellationToken ct = default)
    {
        return context.Invoices.AnyAsync(
            i => i.VendorId == vendorId && i.InvoiceNumber == invoiceNumber, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await context.SaveChangesAsync(ct);
    }
}