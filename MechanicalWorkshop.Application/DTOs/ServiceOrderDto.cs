namespace MechanicalWorkshop.Application.DTOs;

public record CreateServiceOrderRequest(
    Guid CustomerId,
    Guid VehicleId,
    string? Notes,
    List<ServiceOrderServiceItem>? Services,
    List<ServiceOrderPartItem>? Parts
);

public record ServiceOrderServiceItem(
    Guid ServiceId,
    int Quantity = 1
);

public record ServiceOrderPartItem(
    Guid PartId,
    int Quantity
);

public record AddItemsRequest(
    List<ServiceOrderServiceItem>? Services,
    List<ServiceOrderPartItem>? Parts
);

public record UpdateStatusRequest(
    string Status
);

public record ServiceOrderResponse(
    Guid Id,
    string OrderNumber,
    Guid CustomerId,
    string? CustomerName,
    Guid VehicleId,
    string? VehicleLicensePlate,
    string Status,
    string? Notes,
    decimal TotalAmount,
    List<ServiceOrderItemResponse> Items,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? StartedAt,
    DateTime? FinishedAt,
    DateTime? DeliveredAt
);

public record ServiceOrderItemResponse(
    Guid Id,
    string Description,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    Guid? ServiceId,
    Guid? PartId
);

public record ServiceOrderSummaryResponse(
    Guid Id,
    string OrderNumber,
    string? CustomerName,
    string? VehicleLicensePlate,
    string Status,
    decimal TotalAmount,
    DateTime CreatedAt
);

public record AverageExecutionTimeResponse(
    double AverageMinutes
);