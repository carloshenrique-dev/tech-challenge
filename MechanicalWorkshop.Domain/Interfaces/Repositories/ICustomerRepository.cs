using MechanicalWorkshop.Domain.Entities;

namespace MechanicalWorkshop.Domain.Interfaces.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByDocumentAsync(string document);
    Task<bool> ExistsByDocumentAsync(string document);
    Task<IEnumerable<Customer>> SearchByNameAsync(string name);
}