using InvoiceProcessor.Domain.Categories;
using InvoiceProcessor.Domain.Invoices;

namespace InvoiceProcessor.UnitTests.Invoices;

public class InvoiceLineItemTests
{
    [Fact]
    public void Constructor_ComputesLineTotalFromQuantityAndUnitPrice()
    {
        var lineItem = new InvoiceLineItem("Laptop", quantity: 2, unitPrice: 899.99m, vatRate: 0.21m);
        
        Assert.Equal(1799.98m, lineItem.LineTotal);
    }

    [Fact]
    public void Classify_SetCategoryAndId()
    {
        var lineItem = new InvoiceLineItem("Laptop", 1, 899.99m, 0.21m);
        var category = new Category("Hardware");
        
        lineItem.Classify(category);
        
        Assert.Equal(category.Id, lineItem.CategoryId);
        Assert.Same(category, lineItem.Category);
    }
}