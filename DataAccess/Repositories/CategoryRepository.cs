using Domain.Entities;
using DataAccess.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using DataAccess.IRepositories;

namespace DataAccess.Repositories;

public class CategoryRepository(ApplicationDbContext db) : ICategoryRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task AddAsync(Category entity)
    {
        await _db.Categories.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category entity)
    {
        _db.Categories.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _db.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _db.Categories.FindAsync(id);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _db.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task UpdateAsync(Category entity)
    {
        _db.Categories.Update(entity);
        await _db.SaveChangesAsync();
    }
}
