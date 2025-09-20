using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using OpenLib.Application.DTOs;
using OpenLib.Application.Services.Implementations;
using OpenLib.Domain.Exceptions;
using OpenLib.Infrastructure.Persistence;
using OpenLib.Infrastructure.Repositories;
using OpenLib.Infrastructure.UnitOfWork;

namespace OpenLib.UnitTests.Application;

public class EmprestimoServiceTests
{
    [Fact]
    public async Task SolicitarAsync_DeveCriarEmprestimoEAtualizarLivro()
    {
        await using var context = CriarContexto();
        var livro = Domain.Entities.Livro.Criar("Microservices", "Autor", 2019, 1);
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        var service = CriarService(context);
        var request = new SolicitarEmprestimoRequest(livro.Id, DateTime.UtcNow);

        var emprestimo = await service.SolicitarAsync(request, CancellationToken.None);

        emprestimo.Status.Should().Be(Domain.Enums.EmprestimoStatus.Ativo);
        (await context.Livros.SingleAsync()).QuantidadeDisponivel.Should().Be(0);
    }

    [Fact]
    public async Task DevolverAsync_DeveAtualizarStatusEQuantidade()
    {
        await using var context = CriarContexto();
        var livro = Domain.Entities.Livro.Criar("Hexagonal Architecture", "Autor", 2020, 2);
        context.Livros.Add(livro);
        var emprestimo = Domain.Entities.Emprestimo.Solicitar(livro, DateTime.UtcNow);
        context.Emprestimos.Add(emprestimo);
        await context.SaveChangesAsync();

        var service = CriarService(context);

        var resultado = await service.DevolverAsync(emprestimo.Id, new DevolverEmprestimoRequest(DateTime.UtcNow), CancellationToken.None);

        resultado.Status.Should().Be(Domain.Enums.EmprestimoStatus.Devolvido);
        (await context.Livros.SingleAsync()).QuantidadeDisponivel.Should().Be(2);
    }

    [Fact]
    public async Task SolicitarAsync_DeveLancarExcecao_QuandoLivroIndisponivel()
    {
        await using var context = CriarContexto();
        var livro = Domain.Entities.Livro.Criar("Design", "Autor", 2018, 0);
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        var service = CriarService(context);
        var request = new SolicitarEmprestimoRequest(livro.Id, DateTime.UtcNow);

        var acao = () => service.SolicitarAsync(request, CancellationToken.None);

        await acao.Should().ThrowAsync<DomainException>();
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
