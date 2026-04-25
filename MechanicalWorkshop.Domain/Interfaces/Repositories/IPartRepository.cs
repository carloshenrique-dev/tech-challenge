using MechanicalWorkshop.Domain.Entities;

namespace MechanicalWorkshop.Domain.Interfaces.Repositories;

public interface IPartRepository : IRepository<Part>
{
    Task<IEnumerable<Part>> GetActiveAsync();
    Task<IEnumerable<Part>> GetLowStockAsync();
    Task<bool> ExistsByNameAsync(string name);
}