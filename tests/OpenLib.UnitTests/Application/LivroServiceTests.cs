using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using OpenLib.Application.DTOs;
using OpenLib.Application.Services.Implementations;
using OpenLib.Domain.Entities;
using OpenLib.Infrastructure.Persistence;
using OpenLib.Infrastructure.Repositories;
using OpenLib.Infrastructure.UnitOfWork;
using Xunit;

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

        resultado.Id.Should().NotBe(0);
        (await context.Livros.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarLivrosPaginados()
    {
        await using var context = CriarContexto();
        context.Livros.Add(Livro.Criar("Livro 1", "Autor 1", 2000, 2, 1));
        context.Livros.Add(Livro.Criar("Livro 2", "Autor 2", 2001, 3, 2));
        context.Livros.Add(Livro.Criar("Livro 3", "Autor 3", 2002, 4, 3));
        await context.SaveChangesAsync();

        var repository = new LivroRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var service = new LivroService(repository, unitOfWork, NullLogger<LivroService>.Instance);

        var pagina1 = await service.ListarAsync(1, 2, CancellationToken.None);
        var pagina2 = await service.ListarAsync(2, 2, CancellationToken.None);

        pagina1.Should().HaveCount(2);
        pagina2.Should().HaveCount(1);
        pagina1.Select(l => l.Titulo).Should().Contain("Livro 1").And.Contain("Livro 2");
        pagina2.Select(l => l.Titulo).Should().Contain("Livro 3");
    }

    private static LibraryDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new LibraryDbContext(options);
    }
}
