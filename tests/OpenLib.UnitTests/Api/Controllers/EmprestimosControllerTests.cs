using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OpenLib.Api.Controllers;
using OpenLib.Application.DTOs;
using OpenLib.Application.Services;
using Xunit;

namespace OpenLib.UnitTests.Api.Controllers;

public class EmprestimosControllerTests
{
    [Fact]
    public async Task Listar_DeveRetornarBadRequest_QuandoPaginaForInvalida()
    {
        var service = new StubEmprestimoService();
        var controller = new EmprestimosController(service);

        var resultado = await controller.Listar(CancellationToken.None, 0, 10);

        resultado.Result.Should().BeOfType<BadRequestObjectResult>();
        service.ListarFoiChamado.Should().BeFalse();
    }

    [Fact]
    public async Task Listar_DeveRetornarBadRequest_QuandoTamanhoForInvalido()
    {
        var service = new StubEmprestimoService();
        var controller = new EmprestimosController(service);

        var resultado = await controller.Listar(CancellationToken.None, 1, 0);

        resultado.Result.Should().BeOfType<BadRequestObjectResult>();
        service.ListarFoiChamado.Should().BeFalse();
    }

    private sealed class StubEmprestimoService : IEmprestimoService
    {
        public bool ListarFoiChamado { get; private set; }

        public Task<EmprestimoDto> SolicitarAsync(SolicitarEmprestimoRequest request, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<EmprestimoDto> DevolverAsync(int id, DevolverEmprestimoRequest request, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<EmprestimoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<IReadOnlyCollection<EmprestimoDto>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken)
        {
            ListarFoiChamado = true;
            return Task.FromResult<IReadOnlyCollection<EmprestimoDto>>(Array.Empty<EmprestimoDto>());
        }
    }
}
