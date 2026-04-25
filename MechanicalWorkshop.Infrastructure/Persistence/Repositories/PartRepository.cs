using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Interfaces.Repositories;
using MechanicalWorkshop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MechanicalWorkshop.Infrastructure.Persistence.Repositories;

public class PartRepository : BaseRepository<Part>, IPartRepository
{
    public PartRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Part>> GetActiveAsync() =>
        await DbSet.Where(x => x.IsActive).ToListAsync();

    public async Task<IEnumerable<Part>> GetLowStockAsync() =>
        await DbSet.Where(x => x.StockQuantity <= x.MinimumStock).ToListAsync();

    public async Task<bool> ExistsByNameAsync(string name) =>
        await DbSet.AnyAsync(x => x.Name.ToLower() == name.ToLower());
}