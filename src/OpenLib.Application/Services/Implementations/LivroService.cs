using Microsoft.Extensions.Logging;
using OpenLib.Application.Abstractions.Repositories;
using OpenLib.Application.Abstractions.UnitOfWork;
using OpenLib.Application.DTOs;
using OpenLib.Domain.Entities;

namespace OpenLib.Application.Services.Implementations;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _livroRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LivroService> _logger;

    public LivroService(ILivroRepository livroRepository, IUnitOfWork unitOfWork, ILogger<LivroService> logger)
    {
        _livroRepository = livroRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<LivroDto> CriarAsync(CreateLivroRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Criando livro {Titulo}", request.Titulo);
        var livro = Livro.Criar(request.Titulo, request.Autor, request.AnoPublicacao, request.QuantidadeDisponivel);

        await _livroRepository.AdicionarAsync(livro, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return LivroDto.FromEntity(livro);
    }

    public async Task<LivroDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var livro = await _livroRepository.ObterPorIdAsync(id, cancellationToken);
        return livro is null ? null : LivroDto.FromEntity(livro);
    }

    public async Task<IReadOnlyCollection<LivroDto>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken)
    {
        if (pagina < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pagina), "O parâmetro pagina deve ser maior ou igual a 1.");
        }

        if (tamanho < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(tamanho), "O parâmetro tamanho deve ser maior ou igual a 1.");
        }

        var livros = await _livroRepository.ListarAsync(pagina, tamanho, cancellationToken);
        return livros.Select(LivroDto.FromEntity).ToList();
    }
}
