namespace OpenLib.Application.DTOs;

public record CreateLivroRequest(string Titulo, string Autor, int AnoPublicacao, int QuantidadeDisponivel);
