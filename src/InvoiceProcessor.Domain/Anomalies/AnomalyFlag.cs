namespace InvoiceProcessor.Domain.Anomalies;

public class AnomalyFlag
{
    public Guid Id { get; private set; }
    public Guid InvoiceId { get; private set; }
    
    public string Code { get; private set; }
    public string Reason { get; private set; }
    public AnomalySeverity Severity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private AnomalyFlag()
    {
        Code = null!;
        Reason = null!;
    }

    public AnomalyFlag(string code, string reason, AnomalySeverity severity)
    {
        Code = code;
        Reason = reason;
        Severity = severity;
        CreatedAt = DateTime.UtcNow;
    }
}