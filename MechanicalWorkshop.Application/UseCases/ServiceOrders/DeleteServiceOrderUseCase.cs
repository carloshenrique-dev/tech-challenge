using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.ServiceOrders;

public class DeleteServiceOrderUseCase
{
    private readonly IServiceOrderRepository _serviceOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository, IUnitOfWork unitOfWork)
    {
        _serviceOrderRepository = serviceOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var order = await _serviceOrderRepository.GetByIdAsync(id)
            ?? throw new DomainException("Service order not found.");

        await _serviceOrderRepository.DeleteAsync(order);
        await _unitOfWork.CommitAsync();
    }
}