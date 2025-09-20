using FluentValidation;
using OpenLib.Domain.Entities;
using OpenLib.Domain.Enums;

namespace OpenLib.Domain.Validators;

public class EmprestimoDevolucaoValidator : AbstractValidator<Emprestimo>
{
    public EmprestimoDevolucaoValidator()
    {
        RuleFor(e => e.Status)
            .Equal(EmprestimoStatus.Ativo)
            .WithMessage("O empréstimo já foi devolvido.");
    }
}
