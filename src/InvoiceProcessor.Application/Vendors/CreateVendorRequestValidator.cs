using FluentValidation;

namespace InvoiceProcessor.Application.Vendors;

public class CreateVendorRequestValidator : AbstractValidator<CreateVendorRequest>
{
    public CreateVendorRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);

        RuleFor(x => x.VatNumber).MaximumLength(50).When(x => x.VatNumber is not null);
    }
    
}