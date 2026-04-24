using MechanicalWorkshop.Domain.Enums;
using MechanicalWorkshop.Domain.Exceptions;

namespace MechanicalWorkshop.Domain.Entities;

public class ServiceOrder
{
    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public Guid VehicleId { get; private set; }
    public ServiceOrderStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }

    public Customer Customer { get; private set; } = null!;
    public Vehicle Vehicle { get; private set; } = null!;

    public IReadOnlyCollection<ServiceOrderItem> Items => _items.AsReadOnly();
    private readonly List<ServiceOrderItem> _items = [];

    private static readonly Dictionary<ServiceOrderStatus, ServiceOrderStatus[]> _allowedTransitions = new()
    {
        { ServiceOrderStatus.Received,        [ServiceOrderStatus.InDiagnosis] },
        { ServiceOrderStatus.InDiagnosis,     [ServiceOrderStatus.WaitingApproval] },
        { ServiceOrderStatus.WaitingApproval, [ServiceOrderStatus.InExecution] },
        { ServiceOrderStatus.InExecution,     [ServiceOrderStatus.Finished] },
        { ServiceOrderStatus.Finished,        [ServiceOrderStatus.Delivered] },
        { ServiceOrderStatus.Delivered,       [] }
    };

    protected ServiceOrder() { }

    public ServiceOrder(Guid customerId, Guid vehicleId, string? notes = null)
    {
        Id = Guid.NewGuid();
        OrderNumber = GenerateOrderNumber();
        CustomerId = customerId;
        VehicleId = vehicleId;
        Status = ServiceOrderStatus.Received;
        Notes = notes;
        TotalAmount = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddService(Service service, int quantity = 1)
    {
        EnsureCanModifyItems();
        var item = ServiceOrderItem.FromService(Id, service, quantity);
        _items.Add(item);
        RecalculateTotal();
    }

    public void AddPart(Part part, int quantity)
    {
        EnsureCanModifyItems();
        part.RemoveStock(quantity);
        var item = ServiceOrderItem.FromPart(Id, part, quantity);
        _items.Add(item);
        RecalculateTotal();
    }

    public void TransitionTo(ServiceOrderStatus newStatus)
    {
        if (!_allowedTransitions[Status].Contains(newStatus))
            throw new InvalidStatusTransitionException(Status, newStatus);

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        if (newStatus == ServiceOrderStatus.InExecution) StartedAt = DateTime.UtcNow;
        if (newStatus == ServiceOrderStatus.Finished) FinishedAt = DateTime.UtcNow;
        if (newStatus == ServiceOrderStatus.Delivered) DeliveredAt = DateTime.UtcNow;
    }

    public void UpdateNotes(string notes)
    {
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    private void EnsureCanModifyItems()
    {
        if (Status != ServiceOrderStatus.Received && Status != ServiceOrderStatus.InDiagnosis)
            throw new DomainException("Items can only be modified when the order is in Received or InDiagnosis status.");
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(i => i.TotalPrice);
    }

    private static string GenerateOrderNumber()
    {
        return $"OS-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
    }
}