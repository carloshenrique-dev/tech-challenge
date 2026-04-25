using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Interfaces.Repositories;
using MechanicalWorkshop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MechanicalWorkshop.Infrastructure.Persistence.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

    public async Task<Customer?> GetByDocumentAsync(string document) =>
        await DbSet.FirstOrDefaultAsync(x => x.Document == document);

    public async Task<bool> ExistsByDocumentAsync(string document) =>
        await DbSet.AnyAsync(x => x.Document == document);

    public async Task<IEnumerable<Customer>> SearchByNameAsync(string name) =>
        await DbSet.Where(x => EF.Functions.ILike(x.Name, $"%{name}%")).ToListAsync();
}