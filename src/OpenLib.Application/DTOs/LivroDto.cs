using OpenLib.Domain.Entities;

namespace OpenLib.Application.DTOs;

public record LivroDto(Guid Id, string Titulo, string Autor, int AnoPublicacao, int QuantidadeDisponivel)
{
    public static LivroDto FromEntity(Livro livro) =>
        new(livro.Id, livro.Titulo, livro.Autor, livro.AnoPublicacao, livro.QuantidadeDisponivel);
}
