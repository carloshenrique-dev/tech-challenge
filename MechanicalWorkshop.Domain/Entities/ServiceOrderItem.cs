namespace MechanicalWorkshop.Domain.Entities;

public class ServiceOrderItem
{
    public Guid Id { get; private set; }
    public Guid ServiceOrderId { get; private set; }
    public Guid? ServiceId { get; private set; }
    public Guid? PartId { get; private set; }
    public string Description { get; private set; } = null!;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice => UnitPrice * Quantity;

    public Service? Service { get; private set; }
    public Part? Part { get; private set; }

    protected ServiceOrderItem() { }

    public static ServiceOrderItem FromService(Guid serviceOrderId, Service service, int quantity = 1)
    {
        return new ServiceOrderItem
        {
            Id = Guid.NewGuid(),
            ServiceOrderId = serviceOrderId,
            ServiceId = service.Id,
            Description = service.Name,
            Quantity = quantity,
            UnitPrice = service.Price
        };
    }

    public static ServiceOrderItem FromPart(Guid serviceOrderId, Part part, int quantity)
    {
        return new ServiceOrderItem
        {
            Id = Guid.NewGuid(),
            ServiceOrderId = serviceOrderId,
            PartId = part.Id,
            Description = part.Name,
            Quantity = quantity,
            UnitPrice = part.UnitPrice
        };
    }
}