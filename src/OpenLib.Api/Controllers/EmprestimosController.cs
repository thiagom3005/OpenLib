using Microsoft.AspNetCore.Mvc;
using OpenLib.Application.DTOs;
using OpenLib.Application.Services;
using OpenLib.Domain.Exceptions;

namespace OpenLib.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmprestimosController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;

    public EmprestimosController(IEmprestimoService emprestimoService)
    {
        _emprestimoService = emprestimoService;
    }

    [HttpPost]
    public async Task<ActionResult<EmprestimoDto>> Solicitar([FromBody] SolicitarEmprestimoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var emprestimo = await _emprestimoService.SolicitarAsync(request, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = emprestimo.Id }, emprestimo);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { erro = ex.Message });
        }
    }

    [HttpPost("{id:guid}/devolver")]
    public async Task<ActionResult<EmprestimoDto>> Devolver(Guid id, [FromBody] DevolverEmprestimoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var emprestimo = await _emprestimoService.DevolverAsync(id, request, cancellationToken);
            return Ok(emprestimo);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { erro = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmprestimoDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var emprestimo = await _emprestimoService.ObterPorIdAsync(id, cancellationToken);
        return emprestimo is null ? NotFound() : Ok(emprestimo);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<EmprestimoDto>>> Listar(CancellationToken cancellationToken)
    {
        var emprestimos = await _emprestimoService.ListarAsync(cancellationToken);
        return Ok(emprestimos);
    }
}
