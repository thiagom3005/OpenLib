using OpenLib.Application.DTOs;

namespace OpenLib.Application.Services;

public interface ILivroService
{
    Task<LivroDto> CriarAsync(CreateLivroRequest request, CancellationToken cancellationToken);
    Task<LivroDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<LivroDto>> ListarAsync(CancellationToken cancellationToken);
}
