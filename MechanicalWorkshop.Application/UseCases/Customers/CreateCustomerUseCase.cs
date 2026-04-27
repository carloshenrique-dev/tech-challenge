using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Customers;

public class CreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerUseCase(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponse> ExecuteAsync(CreateCustomerRequest request)
    {
        if (await _customerRepository.ExistsByDocumentAsync(request.Document))
            throw new DomainException("A customer with this document already exists.");

        var customer = new Customer(
            request.Name,
            request.Document,
            request.DocumentType,
            request.Email,
            request.Phone
        );

        await _customerRepository.AddAsync(customer);
        await _unitOfWork.CommitAsync();

        return ToResponse(customer);
    }

    private static CustomerResponse ToResponse(Customer c) =>
        new(c.Id, c.Name, c.Document, c.DocumentType, c.Email, c.Phone, c.CreatedAt, c.UpdatedAt);
}