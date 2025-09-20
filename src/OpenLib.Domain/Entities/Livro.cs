using FluentValidation;
using OpenLib.Domain.Exceptions;
using OpenLib.Domain.Validators;

namespace OpenLib.Domain.Entities;

public class Livro
{
    private readonly IValidator<Livro> _validator = new LivroValidator();
    private readonly IValidator<Livro> _emprestimoValidator = new LivroEmprestimoValidator();

    protected Livro()
    {
    }

    private Livro(string titulo, string autor, int anoPublicacao, int quantidadeDisponivel)
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Autor = autor;
        AnoPublicacao = anoPublicacao;
        QuantidadeDisponivel = quantidadeDisponivel;
        Validar();
    }

    public Guid Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Autor { get; private set; } = string.Empty;
    public int AnoPublicacao { get; private set; }
    public int QuantidadeDisponivel { get; private set; }

    public static Livro Criar(string titulo, string autor, int anoPublicacao, int quantidadeDisponivel)
    {
        return new Livro(titulo, autor, anoPublicacao, quantidadeDisponivel);
    }

    public void RegistrarEmprestimo()
    {
        var resultado = _emprestimoValidator.Validate(this);
        if (!resultado.IsValid)
        {
            throw new DomainException(resultado.ToString());
        }

        QuantidadeDisponivel -= 1;
    }

    public void RegistrarDevolucao()
    {
        QuantidadeDisponivel += 1;
    }

    public bool PossuiDisponibilidade() => QuantidadeDisponivel > 0;

    private void Validar()
    {
        var resultado = _validator.Validate(this);
        if (!resultado.IsValid)
        {
            throw new DomainException(resultado.ToString());
        }
    }
}
