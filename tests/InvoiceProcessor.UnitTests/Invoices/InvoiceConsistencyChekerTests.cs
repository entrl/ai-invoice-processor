using InvoiceProcessor.Domain.Invoices;
using InvoiceProcessor.Domain.Vendors;

namespace InvoiceProcessor.UnitTests.Invoices;

public class InvoiceConsistencyChekerTests
{
    private static Invoice CreateInvoice(decimal subtotal = 100m, decimal vatAmount = 21m, decimal total = 121m,
        bool withLineItem = true)
    {
        var invoice = new Invoice(
            new Vendor("Acme Corp"),
            Guid.NewGuid(),
            "INV-001",
            new DateOnly(2026, 10, 1),
            "EUR"
            );

        if (withLineItem)
            invoice.AddLineItem(new InvoiceLineItem("Consulting", 2, 50m, 0.21m));
        
        invoice.SetTotals(subtotal, vatAmount, total);
        return invoice;
    }

    [Fact]
    public void Check_WithConsistentInvoice_ReturnsNoIssues()
    {
        var issues = InvoiceConsistencyChecker.Check(CreateInvoice());
        
        Assert.Empty(issues);
    }

    [Fact]
    public void Check_WhenSubtotalDoesNotMatchLines_ReturnsSubtotalMismatchOnly()
    {
        var invoice = CreateInvoice(subtotal: 90m, vatAmount: 21m, total: 111m);
        var issues = InvoiceConsistencyChecker.Check(invoice);
        var issue = Assert.Single(issues);
        
        Assert.Equal("SUBTOTAL_MISMATCH", issue.Code);
    }

    [Fact]
    public void Check_WhenDifferenceIsExactlyTolerance_ReturnsNoIssues()
    {
        var invoice = CreateInvoice(subtotal: 100.01m, vatAmount: 21m, total: 121.01m);
        
        var issues = InvoiceConsistencyChecker.Check(invoice);
        
        Assert.Empty(issues);
    }
    
    [Fact]
    public void Check_WhenDifferenceExceedsTolerance_ReturnsSubtotalMismatch()
    {
        var invoice = CreateInvoice(subtotal: 100.02m, vatAmount: 21m, total: 121.02m);

        var issues = InvoiceConsistencyChecker.Check(invoice);

        Assert.Contains(issues, i => i.Code == "SUBTOTAL_MISMATCH");
    }

    [Fact]
    public void Check_WhenVatIsRoundedPerLine_StaysWithinTolerance()
    {
        // 3 x 3.33 = 9.99; exact VAT is 2.0979, but the invoice states 2.10.
        var invoice = new Invoice(new Vendor("Acme Corp"), Guid.NewGuid(), "INV-002",
            new DateOnly(2026, 10, 1), "EUR");
        invoice.AddLineItem(new InvoiceLineItem("Pens", 3, 3.33m, 0.21m));
        invoice.SetTotals(9.99m, 2.10m, 12.09m);

        var issues = InvoiceConsistencyChecker.Check(invoice);

        Assert.Empty(issues);
    }

    [Fact]
    public void Check_WithNoLineItems_ReturnsOnlyNoLineItems()
    {
        var invoice = CreateInvoice(withLineItem: false);

        var issues = InvoiceConsistencyChecker.Check(invoice);

        var issue = Assert.Single(issues);
        Assert.Equal("NO_LINE_ITEMS", issue.Code);
    }

    [Fact]
    public void Check_WithSeveralWrongFigures_ReturnsAllIssues()
    {
        var invoice = CreateInvoice(subtotal: 90m, vatAmount: 15m, total: 200m);

        var issues = InvoiceConsistencyChecker.Check(invoice);

        Assert.Equal(3, issues.Count);
        Assert.Contains(issues, i => i.Code == "SUBTOTAL_MISMATCH");
        Assert.Contains(issues, i => i.Code == "TOTAL_MISMATCH");
        Assert.Contains(issues, i => i.Code == "VAT_MISMATCH");
    }
}