using InvoiceProcessor.Application.Categories;
using InvoiceProcessor.Application.Common;
using InvoiceProcessor.Application.Vendors;
using InvoiceProcessor.Domain.Categories;
using NSubstitute;

namespace InvoiceProcessor.UnitTests.Vendors;


public class VendorServiceTests
{
    private readonly IVendorRepository _vendorRepository = Substitute.For<IVendorRepository>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly VendorService _sut;

    public VendorServiceTests()
    {
        _sut = new VendorService(_vendorRepository, _categoryRepository);
    }

    [Fact]
    public async Task CreateAsync_WithoutCategory_SavesVendor()
    {
        var request = new CreateVendorRequest { Name = "Acme" };

        var result = await _sut.CreateAsync(request);
        
        Assert.Equal("Acme", result.Name);
        Assert.Null(result.DefaultCategoryId);
        await _vendorRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WithExistingCategory_SetsDefaultCategory()
    {
        var category = new Category("Office Supplies");
        _categoryRepository.GetByIdAsync(category.Id, Arg.Any<CancellationToken>()).Returns(category);
        var request = new CreateVendorRequest { Name = "Staples", DefaultCategoryId = category.Id };
        
        var result = await _sut.CreateAsync(request);
        
        Assert.Equal("Office Supplies", result.DefaultCategoryName);
    }
    
    [Fact]
    public async Task CreateAsync_WithMissingCategory_ThrowsNotFoundAndDoesNotSave()
    {
        var missingId = Guid.NewGuid();
        _categoryRepository.GetByIdAsync(missingId, Arg.Any<CancellationToken>()).Returns((Category?)null);
        var request = new CreateVendorRequest { Name = "Staples", DefaultCategoryId = missingId };
        
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.CreateAsync(request));
        
        await _vendorRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}