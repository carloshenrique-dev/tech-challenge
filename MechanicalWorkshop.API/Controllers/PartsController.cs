using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Application.UseCases.Parts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicalWorkshop.API.Controllers;

[ApiController]
[Route("api/parts")]
[Authorize]
public class PartsController : ControllerBase
{
    private readonly CreatePartUseCase _createUseCase;
    private readonly GetPartUseCase _getUseCase;
    private readonly UpdatePartUseCase _updateUseCase;
    private readonly DeletePartUseCase _deleteUseCase;

    public PartsController(
        CreatePartUseCase createUseCase,
        GetPartUseCase getUseCase,
        UpdatePartUseCase updateUseCase,
        DeletePartUseCase deleteUseCase)
    {
        _createUseCase = createUseCase;
        _getUseCase = getUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePartRequest request)
    {
        var response = await _createUseCase.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _getUseCase.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _getUseCase.GetAllAsync();
        return Ok(response);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var response = await _getUseCase.GetActiveAsync();
        return Ok(response);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        var response = await _getUseCase.GetLowStockAsync();
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePartRequest request)
    {
        var response = await _updateUseCase.ExecuteAsync(id, request);
        return Ok(response);
    }

    [HttpPatch("{id:guid}/add-stock")]
    public async Task<IActionResult> AddStock(Guid id, [FromBody] UpdateStockRequest request)
    {
        var response = await _updateUseCase.AddStockAsync(id, request);
        return Ok(response);
    }

    [HttpPatch("{id:guid}/remove-stock")]
    public async Task<IActionResult> RemoveStock(Guid id, [FromBody] UpdateStockRequest request)
    {
        var response = await _updateUseCase.RemoveStockAsync(id, request);
        return Ok(response);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var response = await _updateUseCase.DeactivateAsync(id);
        return Ok(response);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var response = await _updateUseCase.ActivateAsync(id);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteUseCase.ExecuteAsync(id);
        return NoContent();
    }
}