using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    // GUIDs FIJOS — nunca usar Guid.NewGuid() en HasData()
    private static readonly Guid ElectronicaId =
        new Guid("a1b2c3d4-0000-0000-0000-000000000001");
    private static readonly Guid RopaId =
        new Guid("a1b2c3d4-0000-0000-0000-000000000002");
    private static readonly Guid HogarId =
        new Guid("a1b2c3d4-0000-0000-0000-000000000003");
    private static readonly Guid DeportesId =
        new Guid("a1b2c3d4-0000-0000-0000-000000000004");

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        // Datos iniciales — GUIDs hardcodeados obligatoriamente
        builder.HasData(
            new { Id = ElectronicaId, Name = "Electrónica" },
            new { Id = RopaId,        Name = "Ropa"        },
            new { Id = HogarId,       Name = "Hogar"       },
            new { Id = DeportesId,    Name = "Deportes"    }
        );
    }
}
