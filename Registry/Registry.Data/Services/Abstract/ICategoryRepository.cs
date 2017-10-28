using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Data.Services.Abstract
{
  [ServiceContract]
  public interface ICategoryRepository
  {
    [OperationContract]
    Task<GetAllCategoriesResult[]> GetAllCategories();

    [OperationContract]
    Task UpdateCategory(Guid id, string name);

    [OperationContract]
    Task CreateCategory(Guid parentId, string name);

    [OperationContract]
    Task DeleteCategory(Guid id);
  }
}
