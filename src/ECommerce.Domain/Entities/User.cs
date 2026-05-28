using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public User(string email, string name, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("El email es obligatorio.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("El password hash es obligatorio.");

        Id           = Guid.NewGuid();
        Email        = email;
        Name         = name;
        PasswordHash = passwordHash;
        CreatedAt    = DateTime.UtcNow;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre es obligatorio.");
        Name = name;
    }

    public void UpdateRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new DomainException("El rol es obligatorio.");
        Role = role;
    }
}