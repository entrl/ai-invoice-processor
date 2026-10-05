using InvoiceProcessor.Api.Extensions;
using InvoiceProcessor.Application.Invoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceProcessor.Api.Controllers;

[ApiController]
[Route("api/invoices")]
[Authorize]
public class InvoicesController(IInvoiceService invoiceService, CreateInvoiceRequestValidator createValidator) : ControllerBase
{
     [HttpGet]
     public async Task<ActionResult<List<InvoiceResponse>>> GetAll(CancellationToken ct = default)
     {
          return Ok(await invoiceService.GetAllAsync(ct));
     }

     [HttpGet("{id:guid}")]
     public async Task<ActionResult<InvoiceResponse>> GetById(Guid id, CancellationToken ct = default)
     {
          var invoice = await invoiceService.GetByIdAsync(id, ct);
          return invoice is null ? NotFound() : Ok(invoice);
     }

     [HttpPost]
     public async Task<ActionResult<InvoiceResponse>> Create(CreateInvoiceRequest request,
          CancellationToken ct = default)
     {
          var validation = await createValidator.ValidateAsync(request, ct);
          if (!validation.IsValid)
          {
               foreach (var error in validation.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
               return ValidationProblem(ModelState);
          }
          
          var invoice = await invoiceService.CreateAsync(User.GetUserId(), request, ct);
          return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
     }
}