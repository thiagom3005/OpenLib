using OpenLib.Application.DTOs;

namespace OpenLib.Application.Services;

public interface IEmprestimoService
{
    Task<EmprestimoDto> SolicitarAsync(SolicitarEmprestimoRequest request, CancellationToken cancellationToken);
    Task<EmprestimoDto> DevolverAsync(int id, DevolverEmprestimoRequest request, CancellationToken cancellationToken);
    Task<EmprestimoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<EmprestimoDto>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken);
}
