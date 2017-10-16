using System;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Data.Services.Abstract
{
  public interface ICategoryRepository
  {
    Task<GetAllCategoriesResult[]> GetAllCategories();

    Task UpdateCategory(Guid id, string name);

    Task CreateCategory(Guid parentId, string name);

    Task DeleteCategory(Guid id);
  }
}
