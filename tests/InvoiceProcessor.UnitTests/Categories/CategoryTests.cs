using InvoiceProcessor.Domain.Categories;

namespace InvoiceProcessor.UnitTests.Categories;

public class CategoryTests
{
    [Fact]
    public void Rename_UpdatesName()
    {
        var category = new Category("Office Supplies");
        
        category.Rename("Office and Stationery");
        
        Assert.Equal("Office and Stationery", category.Name);
    }
}