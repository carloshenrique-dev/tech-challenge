using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Enums;
using MechanicalWorkshop.Domain.Interfaces.Repositories;
using MechanicalWorkshop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MechanicalWorkshop.Infrastructure.Persistence.Repositories;

public class ServiceOrderRepository : BaseRepository<ServiceOrder>, IServiceOrderRepository
{
    public ServiceOrderRepository(AppDbContext context) : base(context) { }

    public async Task<ServiceOrder?> GetByOrderNumberAsync(string orderNumber) =>
        await DbSet.FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);

    public async Task<ServiceOrder?> GetWithDetailsAsync(Guid id) =>
        await DbSet
            .Include(x => x.Customer)
            .Include(x => x.Vehicle)
            .Include(x => x.Items)
                .ThenInclude(x => x.Service)
            .Include(x => x.Items)
                .ThenInclude(x => x.Part)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<ServiceOrder>> GetByCustomerIdAsync(Guid customerId) =>
        await DbSet
            .Include(x => x.Vehicle)
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<ServiceOrder>> GetByStatusAsync(ServiceOrderStatus status) =>
        await DbSet
            .Include(x => x.Customer)
            .Include(x => x.Vehicle)
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public async Task<double> GetAverageExecutionTimeInMinutesAsync()
    {
        var orders = await DbSet
            .Where(x => x.StartedAt != null && x.FinishedAt != null)
            .Select(x => new { x.StartedAt, x.FinishedAt })
            .ToListAsync();

        if (!orders.Any()) return 0;

        return orders.Average(x => (x.FinishedAt!.Value - x.StartedAt!.Value).TotalMinutes);
    }
}