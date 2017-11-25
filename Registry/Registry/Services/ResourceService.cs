using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Registry.Common;
using Registry.Communication;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class ResourceService : IResourceService
  {
    IResourceRepository _resourceRepository = RegistryCommon.Instance.Container.Resolve<IResourceRepository>();

    public async Task<string> UploadToBlob(FileStream file, string name)
    {
      string connectionString = await _resourceRepository.GetCloudBlobConnectionStringAsync();
      CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
      CloudBlobClient serviceClient = account.CreateCloudBlobClient();

      var container = serviceClient.GetContainerReference("registrycontainer");
      CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);

      await blockBlob.UploadFromStreamAsync(file);
      return blockBlob.Uri.ToString();
    }

    public async Task DeleteFromBlob(string url)
    {
      string connectionString = await _resourceRepository.GetCloudBlobConnectionStringAsync();
      CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
      CloudBlobClient serviceClient = account.CreateCloudBlobClient();
      var container = serviceClient.GetContainerReference("registrycontainer");
      var blob = container.GetBlockBlobReference(url.Split('/').Last());
      await blob.DeleteIfExistsAsync();
    }

    public async Task CreateResource(CreateResourceRequest request)
    {
      await _resourceRepository.CreateResourceAsync(request);
    }

    public async Task UpdateResource(UpdateResourceRequest request)
    {
      await _resourceRepository.UpdateResourceAsync(request);
    }

    public async Task<GetAllResourcesResult[]> GetAllResources(int skip, int take)
    {
      var result = await _resourceRepository.GetAllResourcesAsync(skip, take);
      return result;
    }

    public async Task<GetAllResourcesResult[]> GetResources(UseFiltersRequest filter)
    {
      return await _resourceRepository.GetResourcesAsync(filter);
    }

    public async Task<GetResourceDetailsResult> GetResourceDetails(string resourceId)
    {
      return await _resourceRepository.GetResourceDetailsAsync(resourceId);
    }

    public async Task DeleteResource(string id)
    {
      await _resourceRepository.DeleteResourceAsync(id);
    }
  }
}