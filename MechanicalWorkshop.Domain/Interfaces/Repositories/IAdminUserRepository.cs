using MechanicalWorkshop.Domain.Entities;

namespace MechanicalWorkshop.Domain.Interfaces.Repositories;

public interface IAdminUserRepository : IRepository<AdminUser>
{
    Task<AdminUser?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
}