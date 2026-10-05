namespace InvoiceProcessor.Application.Invoices;

public class InvoiceResponse
{
    public Guid Id { get; set; }
    public Guid VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public Guid UploadedById { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? FilePath { get; set; }
    public List<InvoiceLineItemResponse> LineItems { get; set; } = [];
}