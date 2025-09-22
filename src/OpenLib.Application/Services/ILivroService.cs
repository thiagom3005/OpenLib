using OpenLib.Application.DTOs;

namespace OpenLib.Application.Services;

public interface ILivroService
{
    Task<LivroDto> CriarAsync(CreateLivroRequest request, CancellationToken cancellationToken);
    Task<LivroDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<LivroDto>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken);
}
