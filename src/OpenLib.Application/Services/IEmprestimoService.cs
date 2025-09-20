using OpenLib.Application.DTOs;

namespace OpenLib.Application.Services;

public interface IEmprestimoService
{
    Task<EmprestimoDto> SolicitarAsync(SolicitarEmprestimoRequest request, CancellationToken cancellationToken);
    Task<EmprestimoDto> DevolverAsync(Guid id, DevolverEmprestimoRequest request, CancellationToken cancellationToken);
    Task<EmprestimoDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<EmprestimoDto>> ListarAsync(CancellationToken cancellationToken);
}
