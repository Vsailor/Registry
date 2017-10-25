using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Data.Services.Abstract
{
  public interface IResourceRepository
  {
    Task CreateResource(CreateResourceRequest request);

    Task<GetAllResourcesResult[]> GetAllResources();
  }
}
