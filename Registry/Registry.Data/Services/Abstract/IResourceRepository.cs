using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
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
  }
}
