using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Vehicles;

public class CreateVehicleUseCase
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleUseCase(IVehicleRepository vehicleRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleResponse> ExecuteAsync(CreateVehicleRequest request)
    {
        _ = await _customerRepository.GetByIdAsync(request.CustomerId)
            ?? throw new DomainException("Customer not found.");

        if (await _vehicleRepository.ExistsByLicensePlateAsync(request.LicensePlate))
            throw new DomainException("A vehicle with this license plate already exists.");

        var vehicle = new Vehicle(request.LicensePlate, request.Brand, request.Model, request.Year, request.CustomerId);

        await _vehicleRepository.AddAsync(vehicle);
        await _unitOfWork.CommitAsync();

        return ToResponse(vehicle);
    }

    private static VehicleResponse ToResponse(Vehicle v) =>
        new(v.Id, v.LicensePlate, v.Brand, v.Model, v.Year, v.CustomerId, v.CreatedAt, v.UpdatedAt);
}