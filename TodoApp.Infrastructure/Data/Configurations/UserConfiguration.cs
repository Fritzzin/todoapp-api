using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configuracao de chave primaria
        builder.HasKey(user => user.Id);
        
        // Configuracao de propriedades dos campos
        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(user => user.Password)
            .IsRequired()
            .HasMaxLength(255);

        // Configuracao para emails serem unicos
        builder.HasIndex(user => user.Email).IsUnique();
    }
}