namespace InvoiceProcessor.UnitTests;

public class SanityTests
{
    [Fact]
    public void Addition_TwoPlusTwo_ReturnsFour()
    {
        var a = 2;
        var b = 2;
        
        var result = a + b;
        
        Assert.Equal(4, result);
    }
}