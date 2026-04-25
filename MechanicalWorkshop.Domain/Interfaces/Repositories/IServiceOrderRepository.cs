using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Enums;

namespace MechanicalWorkshop.Domain.Interfaces.Repositories;

public interface IServiceOrderRepository : IRepository<ServiceOrder>
{
    Task<ServiceOrder?> GetByOrderNumberAsync(string orderNumber);
    Task<ServiceOrder?> GetWithDetailsAsync(Guid id);
    Task<IEnumerable<ServiceOrder>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<ServiceOrder>> GetByStatusAsync(ServiceOrderStatus status);
    Task<double> GetAverageExecutionTimeInMinutesAsync();
}