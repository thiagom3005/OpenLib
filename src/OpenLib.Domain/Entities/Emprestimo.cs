using FluentValidation;
using OpenLib.Domain.Enums;
using OpenLib.Domain.Exceptions;
using OpenLib.Domain.Validators;

namespace OpenLib.Domain.Entities;

public class Emprestimo
{
    private readonly IValidator<Emprestimo> _validator = new EmprestimoValidator();
    private readonly IValidator<Emprestimo> _devolucaoValidator = new EmprestimoDevolucaoValidator();

    protected Emprestimo()
    {
    }

    private Emprestimo(Guid livroId, DateTime dataEmprestimo)
    {
        Id = Guid.NewGuid();
        LivroId = livroId;
        DataEmprestimo = dataEmprestimo;
        Status = EmprestimoStatus.Ativo;
        Validar();
    }

    public Guid Id { get; private set; }
    public Guid LivroId { get; private set; }
    public Livro Livro { get; private set; } = null!;
    public DateTime DataEmprestimo { get; private set; }
    public DateTime? DataDevolucao { get; private set; }
    public EmprestimoStatus Status { get; private set; }

    public static Emprestimo Solicitar(Livro livro, DateTime dataEmprestimo)
    {
        ArgumentNullException.ThrowIfNull(livro);

        var emprestimo = new Emprestimo(livro.Id, dataEmprestimo)
        {
            Livro = livro
        };

        livro.RegistrarEmprestimo();
        return emprestimo;
    }

    public void Devolver(Livro livro, DateTime dataDevolucao)
    {
        var resultado = _devolucaoValidator.Validate(this);
        if (!resultado.IsValid)
        {
            throw new DomainException(resultado.ToString());
        }

        Status = EmprestimoStatus.Devolvido;
        DataDevolucao = dataDevolucao;
        livro?.RegistrarDevolucao();
    }

    private void Validar()
    {
        var resultado = _validator.Validate(this);
        if (!resultado.IsValid)
        {
            throw new DomainException(resultado.ToString());
        }
    }
}
