using InvoiceProcessor.Application.Vendors;
using InvoiceProcessor.Domain.Vendors;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessor.Infrastructure.Persistence.Repositories;

public class VendorRepository : IVendorRepository
{
    private readonly AppDbContext _context;

    public VendorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Vendor?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Vendors.Include(v => v.DefoultCategory).FirstOrDefaultAsync(v => v.Id == id, ct);
    }

    public async Task<List<Vendor>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Vendors.Include(v => v.DefoultCategory).OrderBy(v => v.Name).ToListAsync(ct);
    }

    public async Task AddAsync(Vendor vendor, CancellationToken ct = default)
    {
        await _context.Vendors.AddAsync(vendor, ct);
    }

    public async Task<bool> ExistsWithNameAsync(string name, CancellationToken ct = default)
    {
        return await _context.Vendors.AnyAsync(v => v.Name == name, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}