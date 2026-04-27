using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Services;

public class DeleteServiceUseCase
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceUseCase(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id)
            ?? throw new DomainException("Service not found.");

        await _serviceRepository.DeleteAsync(service);
        await _unitOfWork.CommitAsync();
    }
}