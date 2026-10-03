using InvoiceProcessor.Application.Vendors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceProcessor.Api.Controllers;

[ApiController]
[Route("api/vendors")]
[Authorize]
public class VendorsController : ControllerBase
{
    private readonly IVendorService _vendorService;
    private readonly CreateVendorRequestValidator _validator;
    
    public VendorsController(IVendorService vendorService, CreateVendorRequestValidator validator)
    {
        _vendorService = vendorService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<List<VendorResponse>>> GetAll(CancellationToken ct = default)
    {
        var vendors = await _vendorService.GetAllAsync(ct);
        return Ok(vendors);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VendorResponse>> GetById(Guid id, CancellationToken ct = default)
    {
        var vendor = await _vendorService.GetByIdAsync(id, ct);
        return vendor is null ? NotFound() : Ok(vendor);
    }

    [HttpPost]
    public async Task<ActionResult<VendorResponse>> Create(CreateVendorRequest request, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return ValidationProblem(ModelState);
        }

        var vendor = await _vendorService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = vendor.Id }, vendor);
    }
    
}