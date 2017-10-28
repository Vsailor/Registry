using System;
using System.Threading.Tasks;
using Registry.Communication;

namespace Registry.Services.Abstract
{
  public interface ICategoryService
  {
    Task<GetAllCategoriesResult[]> GetAllCategories();

    Task UpdateCategory(Guid id, string name);

    Task DeleteCategory(Guid id);

    Task CreateCategory(Guid parentId, string name);
  }
}