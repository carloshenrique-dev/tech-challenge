using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Entities;

namespace MechanicalWorkshop.Application.UseCases.ServiceOrders;

internal static class ServiceOrderMapper
{
    internal static ServiceOrderResponse ToResponse(ServiceOrder order, string? customerName, string? vehiclePlate) =>
        new(
            order.Id,
            order.OrderNumber,
            order.CustomerId,
            customerName,
            order.VehicleId,
            vehiclePlate,
            order.Status.ToString(),
            order.Notes,
            order.TotalAmount,
            order.Items.Select(i => new ServiceOrderItemResponse(
                i.Id,
                i.Description,
                i.Quantity,
                i.UnitPrice,
                i.TotalPrice,
                i.ServiceId,
                i.PartId
            )).ToList(),
            order.CreatedAt,
            order.UpdatedAt,
            order.StartedAt,
            order.FinishedAt,
            order.DeliveredAt
        );

    internal static ServiceOrderSummaryResponse ToSummary(ServiceOrder order) =>
        new(
            order.Id,
            order.OrderNumber,
            order.Customer?.Name,
            order.Vehicle?.LicensePlate,
            order.Status.ToString(),
            order.TotalAmount,
            order.CreatedAt
        );
}