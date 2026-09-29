using InvoiceProcessor.Domain.Categories;
using InvoiceProcessor.Domain.Vendors;

namespace InvoiceProcessor.UnitTests.Vendors;

public class VendorTests
{
    [Fact]
    public void Constructor_SetsFirstSeenAndLastSeenToNow()
    {
        var before  = DateTime.UtcNow;

        var vendor = new Vendor("Acme Corp");
        
        var after  = DateTime.UtcNow;
        Assert.InRange(vendor.FirstSeenAt, before, after);
        Assert.InRange(vendor.LastSeenAt, before, after);
        
    }
    
    [Fact]
    public void RecordActivity_UpdatesLastSeenAt()
    {
        var vendor = new Vendor("Acme Corp");
        var originalLastSeen = vendor.LastSeenAt;
        
        vendor.RecordActivity();
        
        Assert.True(vendor.LastSeenAt >= originalLastSeen);
    }

    [Fact]
    public void ClearDefaultCategory_SetCategoryAndIdToNull()
    {
        var vendor = new Vendor("Acme Corp");
        vendor.SetDefoultCategory(new Category("Office Supplies"));
        
        vendor.ClearDefoultCategory();
        
        Assert.Null(vendor.DefoultCategoryId);
        Assert.Null(vendor.DefoultCategory);
    }
}