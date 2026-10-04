using FluentValidation;

namespace InvoiceProcessor.Application.Categories;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    
    public UpdateCategoryRequestValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
        
    }
}