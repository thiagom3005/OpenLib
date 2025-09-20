using FluentValidation;
using OpenLib.Domain.Entities;

namespace OpenLib.Domain.Validators;

public class LivroValidator : AbstractValidator<Livro>
{
    public LivroValidator()
    {
        RuleFor(l => l.Titulo)
            .NotEmpty().WithMessage("O título do livro é obrigatório.");

        RuleFor(l => l.Autor)
            .NotEmpty().WithMessage("O autor do livro é obrigatório.");

        RuleFor(l => l.AnoPublicacao)
            .GreaterThan(0).WithMessage("O ano de publicação deve ser válido.");

        RuleFor(l => l.QuantidadeDisponivel)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade disponível não pode ser negativa.");
    }
}
