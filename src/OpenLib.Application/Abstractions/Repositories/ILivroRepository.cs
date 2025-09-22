using OpenLib.Domain.Entities;

namespace OpenLib.Application.Abstractions.Repositories;

public interface ILivroRepository
{
    Task AdicionarAsync(Livro livro, CancellationToken cancellationToken);
    Task<Livro?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Livro>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken);
}
