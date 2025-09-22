using FluentAssertions;
using OpenLib.Domain.Entities;
using OpenLib.Domain.Enums;
using OpenLib.Domain.Exceptions;
using Xunit;

namespace OpenLib.UnitTests.Domain;

public class EmprestimoTests
{
    [Fact]
    public void Solicitar_DeveCriarEmprestimo_QuandoLivroDisponivel()
    {
        var livro = Livro.Criar("DDD Quickly", "Eric Evans", 2004, 1, 1);

        var emprestimo = Emprestimo.Solicitar(livro, DateTime.UtcNow);

        emprestimo.Status.Should().Be(EmprestimoStatus.Ativo);
        livro.QuantidadeDisponivel.Should().Be(0);
    }

    [Fact]
    public void Solicitar_DeveLancarExcecao_QuandoLivroIndisponivel()
    {
        var livro = Livro.Criar("Patterns", "GoF", 1994, 0);

        var acao = () => Emprestimo.Solicitar(livro, DateTime.UtcNow);

        acao.Should().Throw<DomainException>();
    }

    [Fact]
    public void Devolver_DeveAtualizarStatusEQuantidade()
    {
        var livro = Livro.Criar("Test", "Autor", 2021, 2, 1);
        var emprestimo = Emprestimo.Solicitar(livro, DateTime.UtcNow);

        emprestimo.Devolver(livro, DateTime.UtcNow);

        emprestimo.Status.Should().Be(EmprestimoStatus.Devolvido);
        livro.QuantidadeDisponivel.Should().Be(2);
    }

    [Fact]
    public void Devolver_DeveLancarExcecao_QuandoEmprestimoJaDevolvido()
    {
        var livro = Livro.Criar("Book", "Autor", 2022, 1, 1);
        var emprestimo = Emprestimo.Solicitar(livro, DateTime.UtcNow);
        emprestimo.Devolver(livro, DateTime.UtcNow);

        var acao = () => emprestimo.Devolver(livro, DateTime.UtcNow);

        acao.Should().Throw<DomainException>();
    }
}
