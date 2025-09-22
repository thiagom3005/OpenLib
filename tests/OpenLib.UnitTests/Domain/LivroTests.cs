using FluentAssertions;
using OpenLib.Domain.Entities;
using OpenLib.Domain.Exceptions;
using Xunit;

namespace OpenLib.UnitTests.Domain;

public class LivroTests
{
    [Fact]
    public void CriarLivro_DeveRetornarInstancia_QuandoDadosValidos()
    {
        var livro = Livro.Criar("Domain-Driven Design", "Eric Evans", 2003, 3);

        livro.Should().NotBeNull();
        livro.Titulo.Should().Be("Domain-Driven Design");
        livro.QuantidadeDisponivel.Should().Be(3);
    }

    [Fact]
    public void CriarLivro_DeveLancarExcecao_QuandoTituloVazio()
    {
        var acao = () => Livro.Criar(string.Empty, "Autor", 2020, 1);

        acao.Should().Throw<DomainException>();
    }

    [Fact]
    public void RegistrarEmprestimo_DeveReduzirQuantidade()
    {
        var livro = Livro.Criar("Clean Architecture", "Robert C. Martin", 2017, 2);

        livro.RegistrarEmprestimo();

        livro.QuantidadeDisponivel.Should().Be(1);
    }

    [Fact]
    public void RegistrarEmprestimo_DeveLancarExcecao_QuandoSemDisponibilidade()
    {
        var livro = Livro.Criar("Refactoring", "Martin Fowler", 1999, 0);

        var acao = () => livro.RegistrarEmprestimo();

        acao.Should().Throw<DomainException>();
    }

    [Fact]
    public void RegistrarDevolucao_DeveAumentarQuantidade()
    {
        var livro = Livro.Criar("Implementing DDD", "Vaughn Vernon", 2013, 0);

        livro.RegistrarDevolucao();

        livro.QuantidadeDisponivel.Should().Be(1);
    }
}
