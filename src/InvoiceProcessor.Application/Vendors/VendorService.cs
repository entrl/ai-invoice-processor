using InvoiceProcessor.Domain.Vendors;

namespace InvoiceProcessor.Application.Vendors;

public class VendorService : IVendorService
{
    private readonly IVendorRepository _vendorRepository;

    public VendorService(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<VendorResponse> CreateAsync(CreateVendorRequest request, CancellationToken ct = default)
    {
        var vendor = new Vendor(request.Name, request.VatNumber);
        
        await _vendorRepository.AddAsync(vendor, ct);
        await _vendorRepository.SaveChangesAsync(ct);
        
        return ToResponse(vendor);
    }

    public async Task<VendorResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id, ct);
        return vendor is null ? null : ToResponse(vendor);
    }

    public async Task<List<VendorResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var vendors = await _vendorRepository.GetAllAsync(ct);
        return vendors.Select(ToResponse).ToList();
    }

    private static VendorResponse ToResponse(Vendor vendor)
    {
        return new VendorResponse
        {
            Id = vendor.Id,
            Name = vendor.Name,
            VatNumber = vendor.VatNumber,
            DefaultCategoryId = vendor.DefoultCategoryId,
            DefaultCategoryName = vendor.DefoultCategory?.Name,
            FirstSeenAt = vendor.FirstSeenAt,
            LastSeenAt = vendor.LastSeenAt
        };
    }
}