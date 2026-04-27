using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Vehicles;

public class UpdateVehicleUseCase
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleUseCase(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleResponse> ExecuteAsync(Guid id, UpdateVehicleRequest request)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id)
            ?? throw new DomainException("Vehicle not found.");

        vehicle.Update(request.Brand, request.Model, request.Year);

        await _vehicleRepository.UpdateAsync(vehicle);
        await _unitOfWork.CommitAsync();

        return ToResponse(vehicle);
    }

    private static VehicleResponse ToResponse(Vehicle v) =>
        new(v.Id, v.LicensePlate, v.Brand, v.Model, v.Year, v.CustomerId, v.CreatedAt, v.UpdatedAt);
}