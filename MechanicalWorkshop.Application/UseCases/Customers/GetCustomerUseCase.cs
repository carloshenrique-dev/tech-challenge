using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Customers;

public class GetCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> GetByIdAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id)
            ?? throw new DomainException("Customer not found.");

        return ToResponse(customer);
    }

    public async Task<CustomerResponse?> GetByDocumentAsync(string document)
    {
        var customer = await _customerRepository.GetByDocumentAsync(document);
        return customer is null ? null : ToResponse(customer);
    }

    public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(ToResponse);
    }

    public async Task<IEnumerable<CustomerResponse>> SearchByNameAsync(string name)
    {
        var customers = await _customerRepository.SearchByNameAsync(name);
        return customers.Select(ToResponse);
    }

    private static CustomerResponse ToResponse(Customer c) =>
        new(c.Id, c.Name, c.Document, c.DocumentType, c.Email, c.Phone, c.CreatedAt, c.UpdatedAt);
}