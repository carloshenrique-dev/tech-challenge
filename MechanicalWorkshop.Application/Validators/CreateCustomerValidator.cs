using FluentValidation;
using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.ValueObjects;

namespace MechanicalWorkshop.Application.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name must be at most 150 characters.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("Document is required.")
            .Must((request, document) => BeAValidDocument(document, request.DocumentType))
            .WithMessage("Invalid document for the specified type.");

        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("Document type is required.")
            .Must(x => x is "CPF" or "CNPJ").WithMessage("Document type must be 'CPF' or 'CNPJ'.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(150).WithMessage("Email must be at most 150 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(20).WithMessage("Phone must be at most 20 characters.");
    }

    private static bool BeAValidDocument(string document, string documentType) =>
        documentType?.ToUpper() switch
        {
            "CPF" => Cpf.IsValid(document),
            "CNPJ" => Cnpj.IsValid(document),
            _ => false
        };
}