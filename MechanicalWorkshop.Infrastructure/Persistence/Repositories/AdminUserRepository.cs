using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Interfaces.Repositories;
using MechanicalWorkshop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MechanicalWorkshop.Infrastructure.Persistence.Repositories;

public class AdminUserRepository : BaseRepository<AdminUser>, IAdminUserRepository
{
    public AdminUserRepository(AppDbContext context) : base(context) { }

    public async Task<AdminUser?> GetByEmailAsync(string email) =>
        await DbSet.FirstOrDefaultAsync(x => x.Email == email.ToLower().Trim());

    public async Task<bool> ExistsByEmailAsync(string email) =>
        await DbSet.AnyAsync(x => x.Email == email.ToLower().Trim());
}