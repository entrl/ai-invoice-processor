using InvoiceProcessor.Domain.Categories;

namespace InvoiceProcessor.Domain.Invoices;

public class InvoiceLineItem
{
    public Guid Id { get; private set; }
    public Guid InvoiceId { get; private set; }
    
    public string Description { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal VatRate { get; private set; }
    public decimal LineTotal { get; private set; }
    
    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    private InvoiceLineItem()
    {
        Description = null!;
    }

    public InvoiceLineItem(string description, decimal quantity, decimal unitPrice, decimal vatRate)
    {
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        VatRate = vatRate;
        LineTotal = quantity * unitPrice;
    }

    public void Classify(Category category)
    {
        CategoryId = category.Id;
        Category = category;
    }
}