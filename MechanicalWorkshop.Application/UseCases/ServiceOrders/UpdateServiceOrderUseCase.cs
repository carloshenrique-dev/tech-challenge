using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Enums;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.ServiceOrders;

public class UpdateServiceOrderUseCase
{
    private readonly IServiceOrderRepository _serviceOrderRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IPartRepository _partRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceOrderUseCase(
        IServiceOrderRepository serviceOrderRepository,
        IServiceRepository serviceRepository,
        IPartRepository partRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceOrderRepository = serviceOrderRepository;
        _serviceRepository = serviceRepository;
        _partRepository = partRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceOrderResponse> UpdateStatusAsync(Guid id, UpdateStatusRequest request)
    {
        if (!Enum.TryParse<ServiceOrderStatus>(request.Status, true, out var newStatus))
            throw new DomainException($"Invalid status: '{request.Status}'.");

        var order = await _serviceOrderRepository.GetWithDetailsAsync(id)
            ?? throw new DomainException("Service order not found.");

        order.TransitionTo(newStatus);

        await _serviceOrderRepository.UpdateAsync(order);
        await _unitOfWork.CommitAsync();

        return ServiceOrderMapper.ToResponse(order, order.Customer?.Name, order.Vehicle?.LicensePlate);
    }

    public async Task<ServiceOrderResponse> AddItemsAsync(Guid id, AddItemsRequest request)
    {
        var order = await _serviceOrderRepository.GetWithDetailsAsync(id)
            ?? throw new DomainException("Service order not found.");

        if (request.Services is not null)
        {
            foreach (var item in request.Services)
            {
                var service = await _serviceRepository.GetByIdAsync(item.ServiceId)
                    ?? throw new DomainException($"Service '{item.ServiceId}' not found.");
                order.AddService(service, item.Quantity);
            }
        }

        if (request.Parts is not null)
        {
            foreach (var item in request.Parts)
            {
                var part = await _partRepository.GetByIdAsync(item.PartId)
                    ?? throw new DomainException($"Part '{item.PartId}' not found.");
                order.AddPart(part, item.Quantity);
            }
        }

        await _serviceOrderRepository.UpdateAsync(order);
        await _unitOfWork.CommitAsync();

        return ServiceOrderMapper.ToResponse(order, order.Customer?.Name, order.Vehicle?.LicensePlate);
    }
}