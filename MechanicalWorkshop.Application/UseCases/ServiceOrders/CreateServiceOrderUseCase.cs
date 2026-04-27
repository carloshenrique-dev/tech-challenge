using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.ServiceOrders;

public class CreateServiceOrderUseCase
{
    private readonly IServiceOrderRepository _serviceOrderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IPartRepository _partRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceOrderUseCase(
        IServiceOrderRepository serviceOrderRepository,
        ICustomerRepository customerRepository,
        IVehicleRepository vehicleRepository,
        IServiceRepository serviceRepository,
        IPartRepository partRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceOrderRepository = serviceOrderRepository;
        _customerRepository = customerRepository;
        _vehicleRepository = vehicleRepository;
        _serviceRepository = serviceRepository;
        _partRepository = partRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceOrderResponse> ExecuteAsync(CreateServiceOrderRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
            ?? throw new DomainException("Customer not found.");

        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId)
            ?? throw new DomainException("Vehicle not found.");

        if (vehicle.CustomerId != customer.Id)
            throw new DomainException("Vehicle does not belong to this customer.");

        var order = new ServiceOrder(request.CustomerId, request.VehicleId, request.Notes);

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

        await _serviceOrderRepository.AddAsync(order);
        await _unitOfWork.CommitAsync();

        return ServiceOrderMapper.ToResponse(order, customer.Name, vehicle.LicensePlate);
    }
}