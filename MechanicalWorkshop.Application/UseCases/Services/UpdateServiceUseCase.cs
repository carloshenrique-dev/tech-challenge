using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Services;

public class UpdateServiceUseCase
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceUseCase(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> ExecuteAsync(Guid id, UpdateServiceRequest request)
    {
        var service = await _serviceRepository.GetByIdAsync(id)
            ?? throw new DomainException("Service not found.");

        service.Update(request.Name, request.Description, request.Price, request.EstimatedMinutes);

        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.CommitAsync();

        return CreateServiceUseCase.ToResponse(service);
    }

    public async Task<ServiceResponse> DeactivateAsync(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id)
            ?? throw new DomainException("Service not found.");

        service.Deactivate();

        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.CommitAsync();

        return CreateServiceUseCase.ToResponse(service);
    }

    public async Task<ServiceResponse> ActivateAsync(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id)
            ?? throw new DomainException("Service not found.");

        service.Activate();

        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.CommitAsync();

        return CreateServiceUseCase.ToResponse(service);
    }
}