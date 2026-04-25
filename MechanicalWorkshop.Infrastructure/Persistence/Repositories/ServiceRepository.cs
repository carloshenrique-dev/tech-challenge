using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Interfaces.Repositories;
using MechanicalWorkshop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MechanicalWorkshop.Infrastructure.Persistence.Repositories;

public class ServiceRepository : BaseRepository<Service>, IServiceRepository
{
    public ServiceRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Service>> GetActiveAsync() =>
        await DbSet.Where(x => x.IsActive).ToListAsync();

    public async Task<bool> ExistsByNameAsync(string name) =>
        await DbSet.AnyAsync(x => x.Name.ToLower() == name.ToLower());
}