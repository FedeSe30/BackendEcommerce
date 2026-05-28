using Microsoft.EntityFrameworkCore;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;

namespace ECommerce.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _ctx;
    public CategoryRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Categories.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Categories.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(Category category, CancellationToken ct = default)
        => await _ctx.Categories.AddAsync(category, ct);
}
