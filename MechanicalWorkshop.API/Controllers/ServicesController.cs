using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Application.UseCases.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicalWorkshop.API.Controllers;

[ApiController]
[Route("api/services")]
[Authorize]
public class ServicesController : ControllerBase
{
    private readonly CreateServiceUseCase _createUseCase;
    private readonly GetServiceUseCase _getUseCase;
    private readonly UpdateServiceUseCase _updateUseCase;
    private readonly DeleteServiceUseCase _deleteUseCase;

    public ServicesController(
        CreateServiceUseCase createUseCase,
        GetServiceUseCase getUseCase,
        UpdateServiceUseCase updateUseCase,
        DeleteServiceUseCase deleteUseCase)
    {
        _createUseCase = createUseCase;
        _getUseCase = getUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceRequest request)
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateServiceRequest request)
    {
        var response = await _updateUseCase.ExecuteAsync(id, request);
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