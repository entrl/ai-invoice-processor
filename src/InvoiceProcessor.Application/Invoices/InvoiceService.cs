using InvoiceProcessor.Application.Common;
using InvoiceProcessor.Application.Vendors;
using InvoiceProcessor.Domain.Invoices;

namespace InvoiceProcessor.Application.Invoices;

public class InvoiceService (IInvoiceRepository invoiceRepository, IVendorRepository vendorRepository) : IInvoiceService
{
    public async Task<InvoiceResponse> CreateAsync(Guid uploadedById, CreateInvoiceRequest request, CancellationToken ct = default)
    {
        var vendor = await vendorRepository.GetByIdAsync(request.VendorId, ct)
                     ?? throw new NotFoundException($"Vendor {request.VendorId} was not found");

        if (await invoiceRepository.ExistsAsync(vendor.Id, request.InvoiceNumber, ct))
            throw new ConflictException(
                $"Vendor '{vendor.Name}' already has an invoice numbered '{request.InvoiceNumber}'.");
        
        var invoice = new Invoice(
            vendor,
            uploadedById,
            request.InvoiceNumber,
            request.IssueDate,
            request.Currency.ToUpperInvariant(),
            request.DueDate);

        foreach (var item in request.LineItems)
            invoice.AddLineItem(new InvoiceLineItem(item.Description, item.Quantity, item.UnitPrice, item.VatRate));
        
        invoice.SetTotals(request.Subtotal, request.VatAmount, request.Total);
        vendor.RecordActivity();
        
        await invoiceRepository.AddAsync(invoice, ct);
        await invoiceRepository.SaveChangesAsync(ct);
        
        return ToResponse(invoice);

    }

    public async Task<InvoiceResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var invoice = await invoiceRepository.GetByIdAsync(id, ct);
        return invoice is null ? null : ToResponse(invoice);
    }

    public async Task<List<InvoiceResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var invoices = await invoiceRepository.GetAllAsync(ct);
        return invoices.Select(ToResponse).ToList();
    }
    
    private static InvoiceResponse ToResponse(Invoice i) => new()
    {
        Id = i.Id,
        VendorId = i.VendorId,
        VendorName = i.Vendor.Name,
        UploadedById = i.UploadedById,
        InvoiceNumber = i.InvoiceNumber,
        IssueDate = i.IssueDate,
        DueDate = i.DueDate,
        Currency = i.Currency,
        Subtotal = i.Subtotal,
        VatAmount = i.VatAmount,
        Total = i.Total,
        Status = i.Status.ToString(),
        FilePath = i.FilePath,
        LineItems = i.LineItems.Select(li => new InvoiceLineItemResponse
        {
            Id = li.Id,
            Description = li.Description,
            Quantity = li.Quantity,
            UnitPrice = li.UnitPrice,
            VatRate = li.VatRate,
            LineTotal = li.LineTotal
        }).ToList()
    };
}