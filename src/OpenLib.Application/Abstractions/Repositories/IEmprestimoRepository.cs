using OpenLib.Domain.Entities;

namespace OpenLib.Application.Abstractions.Repositories;

public interface IEmprestimoRepository
{
    Task AdicionarAsync(Emprestimo emprestimo, CancellationToken cancellationToken);
    Task<Emprestimo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Emprestimo>> ListarAsync(CancellationToken cancellationToken);
}
