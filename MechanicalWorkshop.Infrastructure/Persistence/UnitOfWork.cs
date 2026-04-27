using MechanicalWorkshop.Domain.Interfaces.Repositories;
using MechanicalWorkshop.Infrastructure.Persistence.Context;

namespace MechanicalWorkshop.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync() =>
        await _context.SaveChangesAsync();
}