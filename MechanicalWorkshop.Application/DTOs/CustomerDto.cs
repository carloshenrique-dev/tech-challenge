namespace MechanicalWorkshop.Application.DTOs;

public record CreateCustomerRequest(
    string Name,
    string Document,
    string DocumentType,
    string Email,
    string Phone
);

public record UpdateCustomerRequest(
    string Name,
    string Email,
    string Phone
);

public record CustomerResponse(
    Guid Id,
    string Name,
    string Document,
    string DocumentType,
    string Email,
    string Phone,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);