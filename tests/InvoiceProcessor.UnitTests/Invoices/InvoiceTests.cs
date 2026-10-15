using InvoiceProcessor.Domain.Common;
using InvoiceProcessor.Domain.Invoices;
using InvoiceProcessor.Domain.Vendors;

namespace InvoiceProcessor.UnitTests.Invoices;

public class InvoiceTests
{
    private static Invoice CreateInvoice()
    {
        var vendor = new Vendor("Acme Corp");
        return new Invoice(vendor, uploadedById: Guid.NewGuid(), invoiceNumber: "INV-001", issueDate: new DateOnly(2026, 1, 15), currency: "EUR", filePath: "/uploads/inv-001.pdf");
    }

    private static Invoice CreateInvoiceWithLineItems(decimal subtotal, decimal vatAmount, decimal total)
    {
        var invoice = CreateInvoice();
        invoice.AddLineItem(new InvoiceLineItem("Consulting", 2, 50m, 0.21m));
        invoice.SetTotals(subtotal, vatAmount, total);
        return invoice;
    }

    [Fact]
    public void Constructor_SetsStatusToPending()
    {
        var invoice = CreateInvoice();
        
        Assert.Equal(InvoiceStatus.Pending, invoice.Status);
    }
    
    [Fact]
    public void AddLineItem_AddsToLineItemsCollection()
    {
        var invoice = CreateInvoice();
        var lineItem = new InvoiceLineItem("Laptop", 1, 899.99m, 0.21m);
        
        invoice.AddLineItem(lineItem);
        
        Assert.Single(invoice.LineItems);
        Assert.Contains(lineItem, invoice.LineItems);
    }

    [Fact]
    public void Confirm_ChangedStatusToConfirmed()
    {
        var invoice = CreateInvoice();
        
        invoice.Confirm(Guid.NewGuid());
        
        Assert.Equal(InvoiceStatus.Confirmed, invoice.Status);
    }
    
    [Fact]
    public void Confirm_WhereAlreadyConfirmed_ThrowsDomainException()
    {
        var invoice = CreateInvoice();
        invoice.Confirm(Guid.NewGuid());
        
        Assert.Throws<DomainException>(() => invoice.Confirm(Guid.NewGuid()));
    }

    [Fact]
    public void Flag_ChangesStatusToFlagged()
    {
        var invoice = CreateInvoice();
        
        invoice.Flag();
        
        Assert.Equal(InvoiceStatus.Flagged, invoice.Status);
    }
    
    [Fact]
    public void Confirm_WhenFlagged_ChangesStatusToConfirmed()
    {
        var invoice = CreateInvoice();
        invoice.Flag();
        
        invoice.Confirm(Guid.NewGuid());
        
        Assert.Equal(InvoiceStatus.Confirmed, invoice.Status);
    }

    [Fact]
    public void RunConsistencyCheck_WithConsistentInvoice_StaysPendingWithNoFlags()
    {
        var invoice = CreateInvoiceWithLineItems(subtotal: 100m, vatAmount: 21m, total: 121m);
        
        invoice.RunConsistencyCheck();
        
        Assert.Equal(InvoiceStatus.Pending, invoice.Status);
        Assert.Empty(invoice.AnomalyFlags);
    }

    [Fact]
    public void RunConsistencyCheck_WithInconsistentInvoice_FlagInvoiceAndRecordsReason()
    {
        var invoice = CreateInvoiceWithLineItems(subtotal: 90m, vatAmount: 15m, total: 200m);
        
        invoice.RunConsistencyCheck();
        
        Assert.Equal(InvoiceStatus.Flagged, invoice.Status);
        Assert.Equal(3, invoice.AnomalyFlags.Count);
        Assert.Contains(invoice.AnomalyFlags, f => f.Code == "SUBTOTAL_MISMATCH");
        Assert.Contains(invoice.AnomalyFlags, f => f.Code == "TOTAL_MISMATCH");
        Assert.Contains(invoice.AnomalyFlags, f => f.Code == "VAT_MISMATCH");
    }

    [Fact]
    public void Confirm_RecordsWhoConfirmedAndWhen()
    {
        var invoice = CreateInvoice();
        var reviewerId = Guid.NewGuid();
        var before = DateTime.UtcNow;
        
        invoice.Confirm(reviewerId);
        
        Assert.Equal(reviewerId, invoice.ConfirmedById);
        Assert.InRange(invoice.ConfirmedAt!.Value, before, DateTime.UtcNow);
    }
    
    [Fact]
    public void Flag_WhenConfirmed_ThrowsDomainException()
    {
        var invoice = CreateInvoice();
        invoice.Confirm(Guid.NewGuid());

        Assert.Throws<DomainException>(() => invoice.Flag());
    }
    
}