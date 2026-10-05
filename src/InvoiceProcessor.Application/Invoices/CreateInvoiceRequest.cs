namespace InvoiceProcessor.Application.Invoices;

public class CreateInvoiceRequest
{
    public Guid VendorId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Total { get; set; }
    public List<CreateInvoiceLineItemRequest> LineItems { get; set; } = [];
}