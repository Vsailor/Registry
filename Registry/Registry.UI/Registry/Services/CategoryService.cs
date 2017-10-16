using System;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class CategoryService : ICategoryService
  {
    private ICategoryRepository _categoryRepository;
    public CategoryService()
    {
      _categoryRepository = RegistryCommon.Instance.Container.Resolve<ICategoryRepository>();
    }

    public async Task<GetAllCategoriesResult[]> GetAllCategories()
    {
      return await _categoryRepository.GetAllCategories();
    }

    public async Task UpdateCategory(Guid id, string name)
    {
      await _categoryRepository.UpdateCategory(id, name);
    }

    public async Task DeleteCategory(Guid id)
    {
      await _categoryRepository.DeleteCategory(id);
    }

    public async Task CreateCategory(Guid parentId, string name)
    {
      await _categoryRepository.CreateCategory(parentId, name);
    }
  }
}