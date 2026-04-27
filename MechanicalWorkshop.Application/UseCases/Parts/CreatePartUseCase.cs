using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Parts;

public class CreatePartUseCase
{
    private readonly IPartRepository _partRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePartUseCase(IPartRepository partRepository, IUnitOfWork unitOfWork)
    {
        _partRepository = partRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PartResponse> ExecuteAsync(CreatePartRequest request)
    {
        if (await _partRepository.ExistsByNameAsync(request.Name))
            throw new DomainException("A part with this name already exists.");

        var part = new Part(request.Name, request.Description, request.UnitPrice, request.StockQuantity, request.MinimumStock);

        await _partRepository.AddAsync(part);
        await _unitOfWork.CommitAsync();

        return ToResponse(part);
    }

    internal static PartResponse ToResponse(Part p) =>
        new(p.Id, p.Name, p.Description, p.UnitPrice, p.StockQuantity, p.MinimumStock, p.IsActive, p.IsLowStock(), p.CreatedAt, p.UpdatedAt);
}