namespace MechanicalWorkshop.Application.DTOs;

public record CreatePartRequest(
    string Name,
    string Description,
    decimal UnitPrice,
    int StockQuantity,
    int MinimumStock
);

public record UpdatePartRequest(
    string Name,
    string Description,
    decimal UnitPrice,
    int MinimumStock
);

public record UpdateStockRequest(
    int Quantity
);

public record PartResponse(
    Guid Id,
    string Name,
    string Description,
    decimal UnitPrice,
    int StockQuantity,
    int MinimumStock,
    bool IsActive,
    bool IsLowStock,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);