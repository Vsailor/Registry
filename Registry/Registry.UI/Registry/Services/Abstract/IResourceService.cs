using System.IO;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Services.Abstract
{
  public interface IResourceService
  {
    Task<string> UploadToBlob(FileStream file, string name);

    Task CreateResource(CreateResourceRequest request);

    Task<GetAllResourcesResult[]> GetAllResources();
  }
}