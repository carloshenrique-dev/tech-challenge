using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Enums;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.ServiceOrders;

public class GetServiceOrderUseCase
{
    private readonly IServiceOrderRepository _serviceOrderRepository;

    public GetServiceOrderUseCase(IServiceOrderRepository serviceOrderRepository)
    {
        _serviceOrderRepository = serviceOrderRepository;
    }

    public async Task<ServiceOrderResponse> GetByIdAsync(Guid id)
    {
        var order = await _serviceOrderRepository.GetWithDetailsAsync(id)
            ?? throw new DomainException("Service order not found.");

        return ServiceOrderMapper.ToResponse(order, order.Customer?.Name, order.Vehicle?.LicensePlate);
    }

    public async Task<ServiceOrderResponse?> GetByOrderNumberAsync(string orderNumber)
    {
        var order = await _serviceOrderRepository.GetByOrderNumberAsync(orderNumber);
        if (order is null) return null;

        var detailed = await _serviceOrderRepository.GetWithDetailsAsync(order.Id);
        return detailed is null ? null : ServiceOrderMapper.ToResponse(detailed, detailed.Customer?.Name, detailed.Vehicle?.LicensePlate);
    }

    public async Task<IEnumerable<ServiceOrderSummaryResponse>> GetByCustomerIdAsync(Guid customerId)
    {
        var orders = await _serviceOrderRepository.GetByCustomerIdAsync(customerId);
        return orders.Select(ServiceOrderMapper.ToSummary);
    }

    public async Task<IEnumerable<ServiceOrderSummaryResponse>> GetByStatusAsync(string status)
    {
        if (!Enum.TryParse<ServiceOrderStatus>(status, true, out var parsedStatus))
            throw new DomainException($"Invalid status: '{status}'.");

        var orders = await _serviceOrderRepository.GetByStatusAsync(parsedStatus);
        return orders.Select(ServiceOrderMapper.ToSummary);
    }

    public async Task<IEnumerable<ServiceOrderSummaryResponse>> GetAllAsync()
    {
        var orders = await _serviceOrderRepository.GetAllAsync();
        return orders.Select(ServiceOrderMapper.ToSummary);
    }

    public async Task<AverageExecutionTimeResponse> GetAverageExecutionTimeAsync()
    {
        var avg = await _serviceOrderRepository.GetAverageExecutionTimeInMinutesAsync();
        return new AverageExecutionTimeResponse(avg);
    }
}