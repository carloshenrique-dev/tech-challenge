using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Vehicles;

public class GetVehicleUseCase
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetVehicleUseCase(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<VehicleResponse> GetByIdAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id)
            ?? throw new DomainException("Vehicle not found.");
        return ToResponse(vehicle);
    }

    public async Task<VehicleResponse?> GetByLicensePlateAsync(string licensePlate)
    {
        var vehicle = await _vehicleRepository.GetByLicensePlateAsync(licensePlate);
        return vehicle is null ? null : ToResponse(vehicle);
    }

    public async Task<IEnumerable<VehicleResponse>> GetByCustomerIdAsync(Guid customerId)
    {
        var vehicles = await _vehicleRepository.GetByCustomerIdAsync(customerId);
        return vehicles.Select(ToResponse);
    }

    public async Task<IEnumerable<VehicleResponse>> GetAllAsync()
    {
        var vehicles = await _vehicleRepository.GetAllAsync();
        return vehicles.Select(ToResponse);
    }

    private static VehicleResponse ToResponse(Vehicle v) =>
        new(v.Id, v.LicensePlate, v.Brand, v.Model, v.Year, v.CustomerId, v.CreatedAt, v.UpdatedAt);
}