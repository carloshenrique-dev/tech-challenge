using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Application.UseCases.ServiceOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicalWorkshop.API.Controllers;

[ApiController]
[Route("api/service-orders")]
[Authorize]
public class ServiceOrdersController : ControllerBase
{
    private readonly CreateServiceOrderUseCase _createUseCase;
    private readonly GetServiceOrderUseCase _getUseCase;
    private readonly UpdateServiceOrderUseCase _updateUseCase;
    private readonly DeleteServiceOrderUseCase _deleteUseCase;

    public ServiceOrdersController(
        CreateServiceOrderUseCase createUseCase,
        GetServiceOrderUseCase getUseCase,
        UpdateServiceOrderUseCase updateUseCase,
        DeleteServiceOrderUseCase deleteUseCase)
    {
        _createUseCase = createUseCase;
        _getUseCase = getUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceOrderRequest request)
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

    [HttpGet("number/{orderNumber}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByOrderNumber(string orderNumber)
    {
        var response = await _getUseCase.GetByOrderNumberAsync(orderNumber);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomerId(Guid customerId)
    {
        var response = await _getUseCase.GetByCustomerIdAsync(customerId);
        return Ok(response);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var response = await _getUseCase.GetByStatusAsync(status);
        return Ok(response);
    }

    [HttpGet("average-execution-time")]
    public async Task<IActionResult> GetAverageExecutionTime()
    {
        var response = await _getUseCase.GetAverageExecutionTimeAsync();
        return Ok(response);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        var response = await _updateUseCase.UpdateStatusAsync(id, request);
        return Ok(response);
    }

    [HttpPost("{id:guid}/items")]
    public async Task<IActionResult> AddItems(Guid id, [FromBody] AddItemsRequest request)
    {
        var response = await _updateUseCase.AddItemsAsync(id, request);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteUseCase.ExecuteAsync(id);
        return NoContent();
    }
}