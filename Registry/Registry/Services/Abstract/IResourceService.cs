using System.IO;
using System.Threading.Tasks;
using Registry.Communication;

namespace Registry.Services.Abstract
{
  public interface IResourceService
  {
    Task<string> UploadToBlob(FileStream file, string name);

    Task CreateResource(CreateResourceRequest request);

    Task UpdateResource(UpdateResourceRequest request);

    Task<GetAllResourcesResult[]> GetAllResources(int skip, int take);

    Task<GetAllResourcesResult[]> GetResources(UseFiltersRequest filter);

    Task<GetResourceDetailsResult> GetResourceDetails(string resourceId);
    Task DeleteResource(string id);
    Task DeleteFromBlob(string url);
  }
}