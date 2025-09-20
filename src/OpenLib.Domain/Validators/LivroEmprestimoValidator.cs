using FluentValidation;
using OpenLib.Domain.Entities;

namespace OpenLib.Domain.Validators;

public class LivroEmprestimoValidator : AbstractValidator<Livro>
{
    public LivroEmprestimoValidator()
    {
        RuleFor(l => l.QuantidadeDisponivel)
            .GreaterThan(0)
            .WithMessage("Não há exemplares disponíveis para empréstimo.");
    }
}
