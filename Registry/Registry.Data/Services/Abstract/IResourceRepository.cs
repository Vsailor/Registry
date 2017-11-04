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
    Task<GetAllResourcesResult[]> GetAllResources(int count, int endId);

    [OperationContract]
    string GetCloudBlobConnectionString();

    [OperationContract]
    Task<GetAllResourcesResult[]> GetResources(UseFiltersRequest filter, int count, int endId);
  }
}
