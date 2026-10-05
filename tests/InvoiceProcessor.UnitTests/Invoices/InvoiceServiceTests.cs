using InvoiceProcessor.Application.Common;
using InvoiceProcessor.Application.Invoices;
using InvoiceProcessor.Application.Vendors;
using InvoiceProcessor.Domain.Vendors;
using NSubstitute;

namespace InvoiceProcessor.UnitTests.Invoices;

public class InvoiceServiceTests
{
    private readonly IInvoiceRepository _invoiceRepository = Substitute.For<IInvoiceRepository>();
    private readonly IVendorRepository _vendorRepository = Substitute.For<IVendorRepository>();
    private readonly InvoiceService _sut;

    public InvoiceServiceTests()
    {
        _sut = new InvoiceService(_invoiceRepository, _vendorRepository);
    }

    private static CreateInvoiceRequest CreateRequest(Guid vendorId) => new()
    {
        VendorId = vendorId,
        InvoiceNumber = "INV-001",
        IssueDate = new DateOnly(2026, 10, 1),
        DueDate = new DateOnly(2026, 10, 31),
        Currency = "eur",
        Subtotal = 100m,
        VatAmount = 21m,
        Total = 121m,
        LineItems =
        [
            new CreateInvoiceLineItemRequest
            {
                Description = "Consulting",
                Quantity = 2,
                UnitPrice = 50m,
                VatRate = 0.21m
            }
        ]
    };

    [Fact]
    public async Task CreateAsync_WithMissingVendor_ThrowsNotFoundAndDoesNotSave()
    {
        var vendorId = Guid.NewGuid();
        _vendorRepository.GetByIdAsync(vendorId, Arg.Any<CancellationToken>()).Returns((Vendor?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _sut.CreateAsync(Guid.NewGuid(), CreateRequest(vendorId)));

        await _invoiceRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateInvoiceNumber_ThrowsConflictAndDoesNotSave()
    {
        var vendorId = Guid.NewGuid();
        _vendorRepository.GetByIdAsync(vendorId, Arg.Any<CancellationToken>()).Returns(new Vendor("Acme Corp"));
        _invoiceRepository
            .ExistsAsync(Arg.Any<Guid>(), "INV-001", Arg.Any<CancellationToken>())
            .Returns(true);

        await Assert.ThrowsAsync<ConflictException>(
            () => _sut.CreateAsync(Guid.NewGuid(), CreateRequest(vendorId)));

        await _invoiceRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_BuildsPendingInvoiceForUploader()
    {
        var vendorId = Guid.NewGuid();
        var uploaderId = Guid.NewGuid();
        _vendorRepository.GetByIdAsync(vendorId, Arg.Any<CancellationToken>()).Returns(new Vendor("Acme Corp"));

        var result = await _sut.CreateAsync(uploaderId, CreateRequest(vendorId));

        Assert.Equal(uploaderId, result.UploadedById);
        Assert.Equal("Pending", result.Status);
        Assert.Equal("Acme Corp", result.VendorName);
        var line = Assert.Single(result.LineItems);
        Assert.Equal(100m, line.LineTotal);
        await _invoiceRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_NormalisesCurrencyToUpperCase()
    {
        var vendorId = Guid.NewGuid();
        _vendorRepository.GetByIdAsync(vendorId, Arg.Any<CancellationToken>()).Returns(new Vendor("Acme Corp"));

        var result = await _sut.CreateAsync(Guid.NewGuid(), CreateRequest(vendorId));

        Assert.Equal("EUR", result.Currency);
    }
}