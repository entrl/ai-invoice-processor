namespace InvoiceProcessor.Application.Common;

public class ConflictException(string message) : Exception(message);