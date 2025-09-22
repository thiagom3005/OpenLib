using Microsoft.EntityFrameworkCore;
using OpenLib.Application.Abstractions.Repositories;
using OpenLib.Domain.Entities;
using OpenLib.Infrastructure.Persistence;

namespace OpenLib.Infrastructure.Repositories;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly LibraryDbContext _context;

    public EmprestimoRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Emprestimo emprestimo, CancellationToken cancellationToken)
    {
        await _context.Emprestimos.AddAsync(emprestimo, cancellationToken);
    }

    public Task<Emprestimo?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Emprestimos
            .Include(e => e.Livro)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Emprestimo>> ListarAsync(int pagina, int tamanho, CancellationToken cancellationToken)
    {
        return await _context.Emprestimos
            .AsNoTracking()
            .Include(e => e.Livro)
            .OrderByDescending(e => e.DataEmprestimo)
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .ToListAsync(cancellationToken);
    }
}
