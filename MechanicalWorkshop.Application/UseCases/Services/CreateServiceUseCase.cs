using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;
using ServiceEntity = MechanicalWorkshop.Domain.Entities.Service;

namespace MechanicalWorkshop.Application.UseCases.Services;

public class CreateServiceUseCase
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceUseCase(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> ExecuteAsync(CreateServiceRequest request)
    {
        if (await _serviceRepository.ExistsByNameAsync(request.Name))
            throw new DomainException("A service with this name already exists.");

        var service = new ServiceEntity(request.Name, request.Description, request.Price, request.EstimatedMinutes);

        await _serviceRepository.AddAsync(service);
        await _unitOfWork.CommitAsync();

        return ToResponse(service);
    }

    internal static ServiceResponse ToResponse(ServiceEntity s) =>
        new(s.Id, s.Name, s.Description, s.Price, s.EstimatedMinutes, s.IsActive, s.CreatedAt, s.UpdatedAt);
}