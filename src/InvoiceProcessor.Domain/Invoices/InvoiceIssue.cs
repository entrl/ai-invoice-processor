using InvoiceProcessor.Domain.Anomalies;

namespace InvoiceProcessor.Domain.Invoices;

public sealed record InvoiceIssue(string Code, string Message, AnomalySeverity Severity);