using Domain.Entities;
using Application.IServices;
using DataAccess.IRepositories;
namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task AddAsync(Category category)
    {
        await _repo.AddAsync(category);
    }

    public async Task DeleteAsync(int id)
    {
        var cat = await _repo.GetByIdAsync(id);
        if (cat is not null)
            await _repo.DeleteAsync(cat);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task UpdateAsync(Category category)
    {
        await _repo.UpdateAsync(category);
    }
}
