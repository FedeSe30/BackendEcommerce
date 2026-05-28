using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Category() { }  // para EF Core

    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre de la categoría es obligatorio.");

        Id   = Guid.NewGuid();
        Name = name;
    }
}
