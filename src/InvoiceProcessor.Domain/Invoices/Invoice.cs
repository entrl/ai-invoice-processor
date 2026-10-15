using InvoiceProcessor.Domain.Anomalies;
using InvoiceProcessor.Domain.Common;
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
    
    public Guid? ConfirmedById { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }

    private readonly List<AnomalyFlag> _anomalyFlags = [];
    private readonly List<InvoiceLineItem> _lineItems = [];
    
    public IReadOnlyList<AnomalyFlag> AnomalyFlags => _anomalyFlags.AsReadOnly();
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

    public void Confirm(Guid confirmedById)
    {
        if (Status == InvoiceStatus.Confirmed)
            throw new DomainException("Invoice already confirmed");
        
        Status = InvoiceStatus.Confirmed;
        ConfirmedById = confirmedById;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void Flag()
    {
        if (Status == InvoiceStatus.Confirmed)
            throw new DomainException("A confirmed invoice cannot be flagged.");
        
        Status = InvoiceStatus.Flagged;
    }

    public void RunConsistencyCheck()
    {
        var issues = InvoiceConsistencyChecker.Check(this);
        
        foreach (var issue in issues)
            _anomalyFlags.Add(new AnomalyFlag(issue.Code, issue.Message, issue.Severity));
        
        if (issues.Count > 0)
            Flag();
    }
}