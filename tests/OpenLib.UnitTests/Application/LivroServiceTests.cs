using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using OpenLib.Application.DTOs;
using OpenLib.Application.Services.Implementations;
using OpenLib.Infrastructure.Persistence;
using OpenLib.Infrastructure.Repositories;
using OpenLib.Infrastructure.UnitOfWork;

namespace OpenLib.UnitTests.Application;

public class LivroServiceTests
{
    [Fact]
    public async Task CriarAsync_DevePersistirLivro()
    {
        await using var context = CriarContexto();
        var repository = new LivroRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var service = new LivroService(repository, unitOfWork, NullLogger<LivroService>.Instance);

        var request = new CreateLivroRequest("Código Limpo", "Robert C. Martin", 2008, 5);

        var resultado = await service.CriarAsync(request, CancellationToken.None);

        resultado.Id.Should().NotBeEmpty();
        (await context.Livros.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarLivros()
    {
        await using var context = CriarContexto();
        context.Livros.Add(Domain.Entities.Livro.Criar("Livro 1", "Autor 1", 2000, 2));
        context.Livros.Add(Domain.Entities.Livro.Criar("Livro 2", "Autor 2", 2001, 3));
        await context.SaveChangesAsync();

        var repository = new LivroRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var service = new LivroService(repository, unitOfWork, NullLogger<LivroService>.Instance);

        var livros = await service.ListarAsync(CancellationToken.None);

        livros.Should().HaveCount(2);
    }

    private static LibraryDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new LibraryDbContext(options);
    }
}
