using FluentValidation;
using MechanicalWorkshop.Application.DTOs;

namespace MechanicalWorkshop.Application.Validators;

public class CreateServiceOrderValidator : AbstractValidator<CreateServiceOrderRequest>
{
    public CreateServiceOrderValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Vehicle ID is required.");

        RuleFor(x => x)
            .Must(x => (x.Services is not null && x.Services.Count > 0) || (x.Parts is not null && x.Parts.Count > 0))
            .WithMessage("At least one service or part must be included.");

        RuleForEach(x => x.Services).ChildRules(s =>
        {
            s.RuleFor(x => x.ServiceId).NotEmpty().WithMessage("Service ID is required.");
            s.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }).When(x => x.Services is not null);

        RuleForEach(x => x.Parts).ChildRules(p =>
        {
            p.RuleFor(x => x.PartId).NotEmpty().WithMessage("Part ID is required.");
            p.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }).When(x => x.Parts is not null);
    }
}