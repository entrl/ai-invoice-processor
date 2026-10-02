namespace InvoiceProcessor.Application.Vendors;

public class VendorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? VatNumber { get; set; }
    public Guid? DefaultCategoryId { get; set; }
    public string? DefaultCategoryName { get; set; }
    public DateTime FirstSeenAt { get; set; }
    public DateTime LastSeenAt { get; set; }
}