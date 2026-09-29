namespace InvoiceProcessor.Domain.Categories;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private Category()
    {
        Name = null!;
    }

    public Category(string name)
    {
        Name = name;
    }
    
    public void Rename(string name)
    {
        Name = name;
    }
}