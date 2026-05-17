using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IServices;

public interface ICategoryService
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}
