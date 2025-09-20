using OpenLib.Domain.Entities;
using OpenLib.Domain.Enums;

namespace OpenLib.Application.DTOs;

public record EmprestimoDto(Guid Id, Guid LivroId, DateTime DataEmprestimo, DateTime? DataDevolucao, EmprestimoStatus Status)
{
    public static EmprestimoDto FromEntity(Emprestimo emprestimo) =>
        new(emprestimo.Id, emprestimo.LivroId, emprestimo.DataEmprestimo, emprestimo.DataDevolucao, emprestimo.Status);
}
