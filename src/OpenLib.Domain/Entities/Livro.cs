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
        Titulo = titulo;
        Autor = autor;
        AnoPublicacao = anoPublicacao;
        QuantidadeDisponivel = quantidadeDisponivel;
        Validar();
    }

    public int Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Autor { get; private set; } = string.Empty;
    public int AnoPublicacao { get; private set; }
    public int QuantidadeDisponivel { get; private set; }

    public static Livro Criar(string titulo, string autor, int anoPublicacao, int quantidadeDisponivel, int id = 0)
    {
        var livro = new Livro(titulo, autor, anoPublicacao, quantidadeDisponivel);
        if (id > 0)
        {
            livro.Id = id;
        }
        livro.Validar();
        return livro;
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
