using OpenLib.Domain.Entities;

namespace OpenLib.Application.Abstractions.Repositories;

public interface IEmprestimoRepository
{
    Task AdicionarAsync(Emprestimo emprestimo, CancellationToken cancellationToken);
    Task<Emprestimo?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Emprestimo>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken);
}
