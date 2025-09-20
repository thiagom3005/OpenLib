using FluentValidation;
using OpenLib.Domain.Entities;

namespace OpenLib.Domain.Validators;

public class EmprestimoValidator : AbstractValidator<Emprestimo>
{
    public EmprestimoValidator()
    {
        RuleFor(e => e.LivroId)
            .NotEmpty().WithMessage("O empréstimo deve estar associado a um livro.");

        RuleFor(e => e.DataEmprestimo)
            .Must(data => data != default)
            .WithMessage("A data de empréstimo deve ser válida.");
    }
}
