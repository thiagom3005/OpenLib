using Microsoft.EntityFrameworkCore;
using OpenLib.Application.Abstractions.Repositories;
using OpenLib.Domain.Entities;
using OpenLib.Infrastructure.Persistence;

namespace OpenLib.Infrastructure.Repositories;

public class LivroRepository : ILivroRepository
{
    private readonly LibraryDbContext _context;

    public LivroRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Livro livro, CancellationToken cancellationToken)
    {
        await _context.Livros.AddAsync(livro, cancellationToken);
    }

    public Task<Livro?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Livros.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Livro>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _context.Livros.AsNoTracking().OrderBy(l => l.Titulo).ToListAsync(cancellationToken);
    }
}
