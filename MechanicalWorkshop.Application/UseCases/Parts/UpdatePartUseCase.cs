using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Parts;

public class UpdatePartUseCase
{
    private readonly IPartRepository _partRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePartUseCase(IPartRepository partRepository, IUnitOfWork unitOfWork)
    {
        _partRepository = partRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PartResponse> ExecuteAsync(Guid id, UpdatePartRequest request)
    {
        var part = await _partRepository.GetByIdAsync(id)
            ?? throw new DomainException("Part not found.");

        part.Update(request.Name, request.Description, request.UnitPrice, request.MinimumStock);

        await _partRepository.UpdateAsync(part);
        await _unitOfWork.CommitAsync();

        return CreatePartUseCase.ToResponse(part);
    }

    public async Task<PartResponse> AddStockAsync(Guid id, UpdateStockRequest request)
    {
        var part = await _partRepository.GetByIdAsync(id)
            ?? throw new DomainException("Part not found.");

        part.AddStock(request.Quantity);

        await _partRepository.UpdateAsync(part);
        await _unitOfWork.CommitAsync();

        return CreatePartUseCase.ToResponse(part);
    }

    public async Task<PartResponse> RemoveStockAsync(Guid id, UpdateStockRequest request)
    {
        var part = await _partRepository.GetByIdAsync(id)
            ?? throw new DomainException("Part not found.");

        part.RemoveStock(request.Quantity);

        await _partRepository.UpdateAsync(part);
        await _unitOfWork.CommitAsync();

        return CreatePartUseCase.ToResponse(part);
    }

    public async Task<PartResponse> DeactivateAsync(Guid id)
    {
        var part = await _partRepository.GetByIdAsync(id)
            ?? throw new DomainException("Part not found.");

        part.Deactivate();

        await _partRepository.UpdateAsync(part);
        await _unitOfWork.CommitAsync();

        return CreatePartUseCase.ToResponse(part);
    }

    public async Task<PartResponse> ActivateAsync(Guid id)
    {
        var part = await _partRepository.GetByIdAsync(id)
            ?? throw new DomainException("Part not found.");

        part.Activate();

        await _partRepository.UpdateAsync(part);
        await _unitOfWork.CommitAsync();

        return CreatePartUseCase.ToResponse(part);
    }
}