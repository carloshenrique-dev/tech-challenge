using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Application.UseCases.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicalWorkshop.API.Controllers;

[ApiController]
[Route("api/vehicles")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly CreateVehicleUseCase _createUseCase;
    private readonly GetVehicleUseCase _getUseCase;
    private readonly UpdateVehicleUseCase _updateUseCase;
    private readonly DeleteVehicleUseCase _deleteUseCase;

    public VehiclesController(
        CreateVehicleUseCase createUseCase,
        GetVehicleUseCase getUseCase,
        UpdateVehicleUseCase updateUseCase,
        DeleteVehicleUseCase deleteUseCase)
    {
        _createUseCase = createUseCase;
        _getUseCase = getUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
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

    [HttpGet("plate/{licensePlate}")]
    public async Task<IActionResult> GetByLicensePlate(string licensePlate)
    {
        var response = await _getUseCase.GetByLicensePlateAsync(licensePlate);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomerId(Guid customerId)
    {
        var response = await _getUseCase.GetByCustomerIdAsync(customerId);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request)
    {
        var response = await _updateUseCase.ExecuteAsync(id, request);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteUseCase.ExecuteAsync(id);
        return NoContent();
    }
}