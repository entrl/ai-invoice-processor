namespace InvoiceProcessor.Application.Vendors;

public class CreateVendorRequest
{
    public string Name { get; set; } = string.Empty;
    public string? VatNumber { get; set; }
    public Guid? DefaultCategoryId { get; set; }
}