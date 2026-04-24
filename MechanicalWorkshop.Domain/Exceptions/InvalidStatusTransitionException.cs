using MechanicalWorkshop.Domain.Enums;

namespace MechanicalWorkshop.Domain.Exceptions;

public class InvalidStatusTransitionException : DomainException
{
    public InvalidStatusTransitionException(ServiceOrderStatus from, ServiceOrderStatus to)
        : base($"Invalid status transition from '{from}' to '{to}'.") { }
}