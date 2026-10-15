using InvoiceProcessor.Domain.Anomalies;

namespace InvoiceProcessor.Domain.Invoices;

public class InvoiceConsistencyChecker
{
    public const decimal DefaultTolerance = 0.01m;

    public static IReadOnlyList<InvoiceIssue> Check(Invoice invoice, decimal tolerance = DefaultTolerance)
    {
        var issues = new List<InvoiceIssue>();

        if (invoice.LineItems.Count == 0)
        {
            issues.Add(new InvoiceIssue("NO_LINE_ITEMS", "The invoice has no line items.", AnomalySeverity.High));
            return issues;
        }

        var lineSum = invoice.LineItems.Sum(li => li.LineTotal);
        if(Differs(lineSum, invoice.Subtotal, tolerance))
            issues.Add(new InvoiceIssue("SUBTOTAL_MISMATCH", $"Line items add up to {lineSum}, but the subtotal is {invoice.Subtotal}.", AnomalySeverity.High));

        var expectedTotal = invoice.Subtotal + invoice.VatAmount;
        if(Differs(expectedTotal, invoice.Total, tolerance))
            issues.Add(new InvoiceIssue("TOTAL_MISMATCH", $"Subtotal plus Vat is {expectedTotal}, but the total is {invoice.Total}.", AnomalySeverity.High));
        
        var lineVat = invoice.LineItems.Sum(li => li.LineTotal * li.VatRate);
        if(Differs(lineVat, invoice.VatAmount, tolerance))
            issues.Add(new InvoiceIssue("VAT_MISMATCH",
                $"Line-item VAT adds up to {Math.Round(lineVat, 2)}, but the VAT amount is {invoice.VatAmount}.", AnomalySeverity.Medium));

        return issues;
    }
    
    private static bool Differs(decimal a, decimal b, decimal tolerance)
        => Math.Abs(a - b) > tolerance;
}