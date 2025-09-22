using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OpenLib.Api.Controllers;
using OpenLib.Application.DTOs;
using OpenLib.Application.Services;
using Xunit;

namespace OpenLib.UnitTests.Api.Controllers;

public class LivrosControllerTests
{
    [Fact]
    public async Task Listar_DeveRetornarBadRequest_QuandoPaginaForInvalida()
    {
        var service = new StubLivroService();
        var controller = new LivrosController(service);

        var resultado = await controller.Listar(CancellationToken.None, 0, 10);

        resultado.Result.Should().BeOfType<BadRequestObjectResult>();
        service.ListarFoiChamado.Should().BeFalse();
    }

    [Fact]
    public async Task Listar_DeveRetornarBadRequest_QuandoTamanhoForInvalido()
    {
        var service = new StubLivroService();
        var controller = new LivrosController(service);

        var resultado = await controller.Listar(CancellationToken.None, 1, 0);

        resultado.Result.Should().BeOfType<BadRequestObjectResult>();
        service.ListarFoiChamado.Should().BeFalse();
    }

    private sealed class StubLivroService : ILivroService
    {
        public bool ListarFoiChamado { get; private set; }

        public Task<LivroDto> CriarAsync(CreateLivroRequest request, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<LivroDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<IReadOnlyCollection<LivroDto>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken)
        {
            ListarFoiChamado = true;
            return Task.FromResult<IReadOnlyCollection<LivroDto>>(Array.Empty<LivroDto>());
        }
    }
}
