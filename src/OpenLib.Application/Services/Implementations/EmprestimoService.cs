using Microsoft.Extensions.Logging;
using OpenLib.Application.Abstractions.Repositories;
using OpenLib.Application.Abstractions.UnitOfWork;
using OpenLib.Application.DTOs;
using OpenLib.Domain.Entities;

namespace OpenLib.Application.Services.Implementations;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository _emprestimoRepository;
    private readonly ILivroRepository _livroRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmprestimoService> _logger;

    public EmprestimoService(
        IEmprestimoRepository emprestimoRepository,
        ILivroRepository livroRepository,
        IUnitOfWork unitOfWork,
        ILogger<EmprestimoService> logger)
    {
        _emprestimoRepository = emprestimoRepository;
        _livroRepository = livroRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<EmprestimoDto> SolicitarAsync(SolicitarEmprestimoRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Solicitando empréstimo para livro {LivroId}", request.LivroId);
        var livro = await _livroRepository.ObterPorIdAsync(request.LivroId, cancellationToken)
            ?? throw new KeyNotFoundException("Livro não encontrado para empréstimo.");

        var emprestimo = Emprestimo.Solicitar(livro, request.DataEmprestimo);
        await _emprestimoRepository.AdicionarAsync(emprestimo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return EmprestimoDto.FromEntity(emprestimo);
    }

    public async Task<EmprestimoDto> DevolverAsync(int id, DevolverEmprestimoRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Devolvendo empréstimo {EmprestimoId}", id);
        var emprestimo = await _emprestimoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Empréstimo não encontrado.");

        var livro = await _livroRepository.ObterPorIdAsync(emprestimo.LivroId, cancellationToken)
            ?? throw new KeyNotFoundException("Livro associado ao empréstimo não encontrado.");

        emprestimo.Devolver(livro, request.DataDevolucao);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return EmprestimoDto.FromEntity(emprestimo);
    }

    public async Task<EmprestimoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var emprestimo = await _emprestimoRepository.ObterPorIdAsync(id, cancellationToken);
        return emprestimo is null ? null : EmprestimoDto.FromEntity(emprestimo);
    }

    public async Task<IReadOnlyCollection<EmprestimoDto>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken)
    {
        if (pagina < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pagina), "O parâmetro pagina deve ser maior ou igual a 1.");
        }

        if (tamanho < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(tamanho), "O parâmetro tamanho deve ser maior ou igual a 1.");
        }

        var emprestimos = await _emprestimoRepository.ListarAsync(pagina, tamanho, cancellationToken);
        return emprestimos.Select(EmprestimoDto.FromEntity).ToList();
    }
}
