using System.IO;
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

    public async Task CreateResource(CreateResourceRequest request)
    {
      await _resourceRepository.CreateResourceAsync(request);
    }

    public async Task<GetAllResourcesResult[]> GetAllResources()
    {
      return await _resourceRepository.GetAllResourcesAsync();
    }
  }
}