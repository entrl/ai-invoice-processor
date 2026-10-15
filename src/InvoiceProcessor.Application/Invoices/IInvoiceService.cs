namespace InvoiceProcessor.Application.Invoices;

public interface IInvoiceService
{
    Task<InvoiceResponse> CreateAsync(Guid uploadedById, CreateInvoiceRequest request, CancellationToken ct = default);
    Task<InvoiceResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<InvoiceResponse>> GetAllAsync(CancellationToken ct = default);
    Task<InvoiceResponse> ConfirmAsync(Guid id, Guid confirmedById, CancellationToken ct = default);
    Task<InvoiceResponse> FlagAsync(Guid id, CancellationToken ct = default);
    
}