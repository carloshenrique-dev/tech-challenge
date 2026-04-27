namespace MechanicalWorkshop.Application.DTOs;

public record CreateVehicleRequest(
    string LicensePlate,
    string Brand,
    string Model,
    int Year,
    Guid CustomerId
);

public record UpdateVehicleRequest(
    string Brand,
    string Model,
    int Year
);

public record VehicleResponse(
    Guid Id,
    string LicensePlate,
    string Brand,
    string Model,
    int Year,
    Guid CustomerId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);