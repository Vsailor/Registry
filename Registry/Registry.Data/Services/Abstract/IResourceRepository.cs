using System.ServiceModel;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Data.Services.Abstract
{
  [ServiceContract]
  public interface IResourceRepository
  {
    [OperationContract]
    Task CreateResource(CreateResourceRequest request);

    [OperationContract]
    Task<GetAllResourcesResult[]> GetAllResources();

    [OperationContract]
    string GetCloudBlobConnectionString();

    [OperationContract]
    Task<GetAllResourcesResult[]> GetResources(UseFiltersRequest filter);

    [OperationContract]
    Task<GetResourceDetailsResult> GetResourceDetails(string resourceId);

    [OperationContract]
    Task UpdateResource(UpdateResourceRequest request);

    [OperationContract]
    Task DeleteResource(string id);
  }
}
