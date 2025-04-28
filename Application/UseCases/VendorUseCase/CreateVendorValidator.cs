using FluentValidation;

namespace AspNetCoreEcommerce.Application.UseCases.VendorUseCase
{
    public class CreateVendorValidator : AbstractValidator<CreateVendorDto>
    {
        public CreateVendorValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Vendor name is required")
                .MinimumLength(3).WithMessage("Vendor name must be at least 3 characters long")
                .MaximumLength(100).WithMessage("Vendor name cannot exceed 100 characters");
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Vendor email is required")
                .EmailAddress().WithMessage("Invalid email format");
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Vendor phone number is required")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Vendor location is required")
                .MinimumLength(3).WithMessage("Vendor location must be at least 3 characters long");

            RuleFor(x => x.Logo)
                .Must(file => file == null || (file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/jpg"))
                .WithMessage("Logo must be a JPG, JPEG or PNG image");
        }
    }
}
