using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DataAccess.IRepositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
}
