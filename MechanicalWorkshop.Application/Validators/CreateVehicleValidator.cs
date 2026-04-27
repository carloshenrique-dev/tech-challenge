using FluentValidation;
using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.ValueObjects;

namespace MechanicalWorkshop.Application.Validators;

public class CreateVehicleValidator : AbstractValidator<CreateVehicleRequest>
{
    public CreateVehicleValidator()
    {
        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License plate is required.")
            .Must(plate => LicensePlate.IsValid(plate))
            .WithMessage("Invalid license plate. Expected format: ABC1D23 (Mercosul) or ABC1234 (old format).");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required.")
            .MaximumLength(50).WithMessage("Brand must be at most 50 characters.");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(100).WithMessage("Model must be at most 100 characters.");

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
            .WithMessage("Year must be between 1900 and next year.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");
    }
}