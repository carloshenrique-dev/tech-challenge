using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Customers;

public class UpdateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerUseCase(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponse> ExecuteAsync(Guid id, UpdateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(id)
            ?? throw new DomainException("Customer not found.");

        customer.Update(request.Name, request.Email, request.Phone);

        await _customerRepository.UpdateAsync(customer);
        await _unitOfWork.CommitAsync();

        return ToResponse(customer);
    }

    private static CustomerResponse ToResponse(Customer c) =>
        new(c.Id, c.Name, c.Document, c.DocumentType, c.Email, c.Phone, c.CreatedAt, c.UpdatedAt);
}