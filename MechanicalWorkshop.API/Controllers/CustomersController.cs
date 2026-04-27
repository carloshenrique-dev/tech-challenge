using MechanicalWorkshop.Application.DTOs;
using MechanicalWorkshop.Application.UseCases.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicalWorkshop.API.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly CreateCustomerUseCase _createUseCase;
    private readonly GetCustomerUseCase _getUseCase;
    private readonly UpdateCustomerUseCase _updateUseCase;
    private readonly DeleteCustomerUseCase _deleteUseCase;

    public CustomersController(
        CreateCustomerUseCase createUseCase,
        GetCustomerUseCase getUseCase,
        UpdateCustomerUseCase updateUseCase,
        DeleteCustomerUseCase deleteUseCase)
    {
        _createUseCase = createUseCase;
        _getUseCase = getUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
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
    public async Task<IActionResult> GetAll([FromQuery] string? name)
    {
        var response = string.IsNullOrWhiteSpace(name)
            ? await _getUseCase.GetAllAsync()
            : await _getUseCase.SearchByNameAsync(name);

        return Ok(response);
    }

    [HttpGet("document/{document}")]
    public async Task<IActionResult> GetByDocument(string document)
    {
        var response = await _getUseCase.GetByDocumentAsync(document);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request)
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