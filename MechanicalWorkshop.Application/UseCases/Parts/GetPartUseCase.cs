using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Parts;

public class GetPartUseCase
{
    private readonly IPartRepository _partRepository;

    public GetPartUseCase(IPartRepository partRepository)
    {
        _partRepository = partRepository;
    }

    public async Task<PartResponse> GetByIdAsync(Guid id)
    {
        var part = await _partRepository.GetByIdAsync(id)
            ?? throw new DomainException("Part not found.");
        return CreatePartUseCase.ToResponse(part);
    }

    public async Task<IEnumerable<PartResponse>> GetAllAsync()
    {
        var parts = await _partRepository.GetAllAsync();
        return parts.Select(CreatePartUseCase.ToResponse);
    }

    public async Task<IEnumerable<PartResponse>> GetActiveAsync()
    {
        var parts = await _partRepository.GetActiveAsync();
        return parts.Select(CreatePartUseCase.ToResponse);
    }

    public async Task<IEnumerable<PartResponse>> GetLowStockAsync()
    {
        var parts = await _partRepository.GetLowStockAsync();
        return parts.Select(CreatePartUseCase.ToResponse);
    }
}