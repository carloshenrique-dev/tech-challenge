using MechanicalWorkshop.Domain.Entities;

namespace MechanicalWorkshop.Domain.Interfaces.Repositories;

public interface IVehicleRepository : IRepository<Vehicle>
{
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
    Task<bool> ExistsByLicensePlateAsync(string licensePlate);
    Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId);
}