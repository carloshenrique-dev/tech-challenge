namespace MechanicalWorkshop.Application.DTOs;

public record CreateServiceRequest(
    string Name,
    string Description,
    decimal Price,
    int EstimatedMinutes
);

public record UpdateServiceRequest(
    string Name,
    string Description,
    decimal Price,
    int EstimatedMinutes
);

public record ServiceResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int EstimatedMinutes,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);