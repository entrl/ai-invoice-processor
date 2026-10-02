namespace InvoiceProcessor.Application.Vendors;

public interface IVendorService
{
    Task<VendorResponse> CreateAsync(CreateVendorRequest request, CancellationToken ct = default);
    Task<VendorResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<VendorResponse>> GetAllAsync(CancellationToken ct = default);
}