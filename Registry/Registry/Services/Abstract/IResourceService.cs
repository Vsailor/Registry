using System.IO;
using System.Threading.Tasks;
using Registry.Communication;

namespace Registry.Services.Abstract
{
  public interface IResourceService
  {
    Task<string> UploadToBlob(FileStream file, string name);

    Task CreateResource(CreateResourceRequest request);

    Task<GetAllResourcesResult[]> GetAllResources(int count, int? endId);

    Task<GetAllResourcesResult[]> GetResources(UseFiltersRequest filter, int count, int endId);
  }
}