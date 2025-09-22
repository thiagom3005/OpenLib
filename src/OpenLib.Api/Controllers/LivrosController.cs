using Microsoft.AspNetCore.Mvc;
using OpenLib.Application.DTOs;
using OpenLib.Application.Services;
using OpenLib.Domain.Exceptions;

namespace OpenLib.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivrosController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivrosController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpPost]
    public async Task<ActionResult<LivroDto>> CriarLivro([FromBody] CreateLivroRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var livro = await _livroService.CriarAsync(request, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = livro.Id }, livro);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LivroDto>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var livro = await _livroService.ObterPorIdAsync(id, cancellationToken);
        return livro is null ? NotFound() : Ok(livro);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<LivroDto>>> Listar(
        CancellationToken cancellationToken,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanho = 10
        )
    {
        var livros = await _livroService.ListarAsync(pagina, tamanho, cancellationToken);
        return Ok(livros);
    }
}
