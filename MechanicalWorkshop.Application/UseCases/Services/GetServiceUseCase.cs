using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;
using ServiceEntity = MechanicalWorkshop.Domain.Entities.Service;

namespace MechanicalWorkshop.Application.UseCases.Services;

public class GetServiceUseCase
{
    private readonly IServiceRepository _serviceRepository;

    public GetServiceUseCase(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ServiceResponse> GetByIdAsync(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id)
            ?? throw new DomainException("Service not found.");
        return CreateServiceUseCase.ToResponse(service);
    }

    public async Task<IEnumerable<ServiceResponse>> GetAllAsync()
    {
        var services = await _serviceRepository.GetAllAsync();
        return services.Select(CreateServiceUseCase.ToResponse);
    }

    public async Task<IEnumerable<ServiceResponse>> GetActiveAsync()
    {
        var services = await _serviceRepository.GetActiveAsync();
        return services.Select(CreateServiceUseCase.ToResponse);
    }
}