using MechanicalWorkshop.Domain.Entities;

namespace MechanicalWorkshop.Domain.Interfaces.Repositories;

public interface IServiceRepository : IRepository<Service>
{
    Task<IEnumerable<Service>> GetActiveAsync();
    Task<bool> ExistsByNameAsync(string name);
}