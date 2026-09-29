using InvoiceProcessor.Domain.Categories;

namespace InvoiceProcessor.Domain.Vendors;

public class Vendor
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? VatNumber { get; private set; }
    public DateTime FirstSeenAt { get; private set; }
    public DateTime LastSeenAt { get; private set; }
    
    public Guid? DefoultCategoryId { get; private set; }
    public Category? DefoultCategory { get; private set; }

    private Vendor()
    {
        Name = null!; 
    }

    public Vendor(string name, string? vatNumber = null)
    {
        Name = name;
        VatNumber = vatNumber;
        FirstSeenAt = DateTime.UtcNow;
        LastSeenAt = DateTime.UtcNow;
    }

    public void RecordActivity()
    {
        LastSeenAt = DateTime.UtcNow;
    }

    public void SetDefoultCategory(Category category)
    {
        DefoultCategoryId = category.Id;
        DefoultCategory = category;
    }

    public void ClearDefoultCategory()
    {
        DefoultCategoryId = null;
        DefoultCategory = null;
    }
}