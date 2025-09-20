using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenLib.Domain.Entities;

namespace OpenLib.Infrastructure.Persistence.Configurations;

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.ToTable("livros");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.Autor)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.AnoPublicacao)
            .IsRequired();

        builder.Property(l => l.QuantidadeDisponivel)
            .IsRequired();
    }
}
