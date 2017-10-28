using System;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Communication;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class CategoryService : ICategoryService
  {
    private ICategoryRepository _categoryRepository = RegistryCommon.Instance.Container.Resolve<ICategoryRepository>();

    public async Task<GetAllCategoriesResult[]> GetAllCategories()
    {
      return await _categoryRepository.GetAllCategoriesAsync();
    }

    public async Task UpdateCategory(Guid id, string name)
    {
      await _categoryRepository.UpdateCategoryAsync(id, name);
    }

    public async Task DeleteCategory(Guid id)
    {
      await _categoryRepository.DeleteCategoryAsync(id);
    }

    public async Task CreateCategory(Guid parentId, string name)
    {
      await _categoryRepository.CreateCategoryAsync(parentId, name);
    }
  }
}