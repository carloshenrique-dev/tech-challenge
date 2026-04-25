using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Interfaces.Repositories;
using MechanicalWorkshop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MechanicalWorkshop.Infrastructure.Persistence.Repositories;

public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(AppDbContext context) : base(context) { }

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate) =>
        await DbSet.FirstOrDefaultAsync(x => x.LicensePlate == licensePlate.ToUpper().Trim());

    public async Task<bool> ExistsByLicensePlateAsync(string licensePlate) =>
        await DbSet.AnyAsync(x => x.LicensePlate == licensePlate.ToUpper().Trim());

    public async Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId) =>
        await DbSet.Where(x => x.CustomerId == customerId).ToListAsync();
}