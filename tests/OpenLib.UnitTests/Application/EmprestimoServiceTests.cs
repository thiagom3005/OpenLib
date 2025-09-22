using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using OpenLib.Application.DTOs;
using OpenLib.Application.Services.Implementations;
using OpenLib.Domain.Entities;
using OpenLib.Domain.Enums;
using OpenLib.Domain.Exceptions;
using OpenLib.Infrastructure.Persistence;
using OpenLib.Infrastructure.Repositories;
using OpenLib.Infrastructure.UnitOfWork;
using Xunit;

namespace OpenLib.UnitTests.Application;

public class EmprestimoServiceTests
{
    [Fact]
    public async Task SolicitarAsync_DeveCriarEmprestimoEAtualizarLivro()
    {
        await using var context = CriarContexto();
        var livro = Livro.Criar("Microservices", "Autor", 2019, 1); // Adjusted to use the correct namespace
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        var service = CriarService(context);
        var request = new SolicitarEmprestimoRequest(livro.Id, DateTime.UtcNow);

        var emprestimo = await service.SolicitarAsync(request, CancellationToken.None);

        emprestimo.Status.Should().Be(EmprestimoStatus.Ativo);
        (await context.Livros.SingleAsync()).QuantidadeDisponivel.Should().Be(0);
    }

    [Fact]
    public async Task DevolverAsync_DeveAtualizarStatusEQuantidade()
    {
        await using var context = CriarContexto();
        var livro = Livro.Criar("Hexagonal Architecture", "Autor", 2020, 2); // Adjusted to use the correct namespace
        context.Livros.Add(livro);
        var emprestimo = Emprestimo.Solicitar(livro, DateTime.UtcNow); // Adjusted to use the correct namespace
        context.Emprestimos.Add(emprestimo);
        await context.SaveChangesAsync();

        var service = CriarService(context);

        var resultado = await service.DevolverAsync(emprestimo.Id, new DevolverEmprestimoRequest(DateTime.UtcNow), CancellationToken.None);

        resultado.Status.Should().Be(EmprestimoStatus.Devolvido);
        (await context.Livros.SingleAsync()).QuantidadeDisponivel.Should().Be(2);
    }

    [Fact]
    public async Task SolicitarAsync_DeveLancarExcecao_QuandoLivroIndisponivel()
    {
        await using var context = CriarContexto();
        var livro = Livro.Criar("Design", "Autor", 2018, 0); // Adjusted to use the correct namespace
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        var service = CriarService(context);
        var request = new SolicitarEmprestimoRequest(livro.Id, DateTime.UtcNow);

        var acao = () => service.SolicitarAsync(request, CancellationToken.None);

        await acao.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarEmprestimosPaginados()
    {
        await using var context = CriarContexto();
        var livro = Livro.Criar("Livro Teste", "Autor", 2020, 10, 1);
        context.Livros.Add(livro);
        context.Emprestimos.Add(Emprestimo.Solicitar(livro, DateTime.UtcNow.AddDays(-3)));
        context.Emprestimos.Add(Emprestimo.Solicitar(livro, DateTime.UtcNow.AddDays(-2)));
        context.Emprestimos.Add(Emprestimo.Solicitar(livro, DateTime.UtcNow.AddDays(-1)));
        await context.SaveChangesAsync();

        var emprestimoRepository = new EmprestimoRepository(context);
        var livroRepository = new LivroRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var service = new EmprestimoService(emprestimoRepository, livroRepository, unitOfWork, NullLogger<EmprestimoService>.Instance);

        var pagina1 = await service.ListarAsync(1, 2, CancellationToken.None);
        var pagina2 = await service.ListarAsync(2, 2, CancellationToken.None);

        pagina1.Should().HaveCount(2);
        pagina2.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, 2)]
    public async Task ListarAsync_DeveLancarExcecao_QuandoParametrosInvalidos(int pagina, int tamanho)
    {
        await using var context = CriarContexto();
        var service = CriarService(context);

        var acao = async () => await service.ListarAsync(pagina, tamanho, CancellationToken.None);

        await acao.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    private static EmprestimoService CriarService(LibraryDbContext context)
    {
        var emprestimoRepository = new EmprestimoRepository(context);
        var livroRepository = new LivroRepository(context);
        var unitOfWork = new UnitOfWork(context);
        return new EmprestimoService(emprestimoRepository, livroRepository, unitOfWork, NullLogger<EmprestimoService>.Instance);
    }

    private static LibraryDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new LibraryDbContext(options);
    }
}
