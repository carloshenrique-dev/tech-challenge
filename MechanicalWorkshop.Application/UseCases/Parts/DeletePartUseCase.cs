using MechanicalWorkshop.Domain.Exceptions;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Parts;

public class DeletePartUseCase
{
    private readonly IPartRepository _partRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePartUseCase(IPartRepository partRepository, IUnitOfWork unitOfWork)
    {
        _partRepository = partRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var part = await _partRepository.GetByIdAsync(id)
            ?? throw new DomainException("Part not found.");

        await _partRepository.DeleteAsync(part);
        await _unitOfWork.CommitAsync();
    }
}