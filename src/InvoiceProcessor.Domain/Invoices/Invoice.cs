using InvoiceProcessor.Domain.Vendors;

namespace InvoiceProcessor.Domain.Invoices;

public class Invoice
{
    public Guid Id { get; private set; }

    public Guid VendorId { get; private set; }
    public Vendor Vendor { get; private set; }

    public Guid UploadedById { get; private set; }

    public string InvoiceNumber { get; private set; }
    public DateOnly IssueDate { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public string Currency { get; private set; }

    public decimal Subtotal { get; private set; }
    public decimal VatAmount { get; private set; }
    public decimal Total { get; private set; }

    public InvoiceStatus Status { get; private set; }
    public string? FilePath { get; private set; }

    private readonly List<InvoiceLineItem> _lineItems = [];
    public IReadOnlyList<InvoiceLineItem> LineItems => _lineItems.AsReadOnly();

    private Invoice()
    {
        Vendor = null!;
        InvoiceNumber = null!;
        Currency = null!;
    }

    public Invoice(
        Vendor vendor,
        Guid uploadedById,
        string invoiceNumber,
        DateOnly issueDate,
        string currency,
        DateOnly? dueDate = null,
        string? filePath = null)
    {
        VendorId = vendor.Id;
        Vendor = vendor;
        UploadedById = uploadedById;
        InvoiceNumber = invoiceNumber;
        IssueDate = issueDate;
        DueDate = dueDate;
        Currency = currency;
        FilePath = filePath;
        Status = InvoiceStatus.Pending;
    }

    public void AddLineItem(InvoiceLineItem lineItem)
    {
        _lineItems.Add(lineItem);
    }

    public void SetTotals(decimal subtotal, decimal vatAmount, decimal total)
    {
        Subtotal = subtotal;
        VatAmount = vatAmount;
        Total = total;
    }

    public void Confirm()
    {
        if (Status == InvoiceStatus.Confirmed)
            throw new InvalidOperationException("Invoice already confirmed");

        Status = InvoiceStatus.Confirmed;
    }

    public void Flag()
    {
        Status = InvoiceStatus.Flagged;
    }
}