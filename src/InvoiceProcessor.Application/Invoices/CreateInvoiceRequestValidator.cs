using FluentValidation;

namespace InvoiceProcessor.Application.Invoices;

public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
    public CreateInvoiceRequestValidator()
    {
        RuleFor(x => x.VendorId).NotEmpty();
        RuleFor(x => x.InvoiceNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Currency).Length(3);

        RuleFor(x => x.DueDate)
            .Must((request, due) => due >= request.IssueDate)
            .When(x => x.DueDate is not null)
            .WithMessage("Due date cannot be before the issue date.");

        RuleFor(x => x.Subtotal).GreaterThanOrEqualTo(0);
        RuleFor(x => x.VatAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Total).GreaterThanOrEqualTo(0);

        RuleFor(x => x.LineItems).NotEmpty();
        RuleForEach(x => x.LineItems).SetValidator(new CreateInvoiceLineItemRequestValidator());
    }
}