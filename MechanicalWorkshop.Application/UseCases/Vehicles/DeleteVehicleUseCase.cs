using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Vehicles;

public class DeleteVehicleUseCase
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleUseCase(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id)
            ?? throw new DomainException("Vehicle not found.");

        await _vehicleRepository.DeleteAsync(vehicle);
        await _unitOfWork.CommitAsync();
    }
}