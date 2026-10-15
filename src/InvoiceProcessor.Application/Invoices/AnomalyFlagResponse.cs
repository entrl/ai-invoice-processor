namespace InvoiceProcessor.Application.Invoices;

public class AnomalyFlagResponse
{
    public string Code { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}