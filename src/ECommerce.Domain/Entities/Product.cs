using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Product : BaseEntity  
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public Guid CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Product() { }

    public Product(string name, string description, decimal price, int stock, Guid categoryId)
        : base(Guid.NewGuid())  // ← el Id lo genera BaseEntity
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre es obligatorio.");
        if (price < 0) throw new DomainException("El precio no puede ser negativo.");
        if (stock < 0) throw new DomainException("El stock no puede ser negativo.");

        Name        = name;
        Description = description;
        Price       = price;
        Stock       = stock;
        CategoryId  = categoryId;
        CreatedAt   = DateTime.UtcNow;
    }

    public void Update(string name, string description, decimal price, int stock, Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre es obligatorio.");
        if (price < 0) throw new DomainException("El precio no puede ser negativo.");
        if (stock < 0) throw new DomainException("El stock no puede ser negativo.");

        Name        = name;
        Description = description;
        Price       = price;
        Stock       = stock;
        CategoryId  = categoryId;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0) throw new DomainException("Precio inválido.");
        Price = newPrice;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity > Stock) throw new DomainException($"Stock insuficiente. Disponible: {Stock}");
        Stock -= quantity;
    }
}