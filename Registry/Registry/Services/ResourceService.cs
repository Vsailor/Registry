using System.IO;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Registry.Common;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class ResourceService : IResourceService
  {
    IResourceRepository _resourceRepository = RegistryCommon.Instance.Container.Resolve<IResourceRepository>();

    public async Task<string> UploadToBlob(FileStream file, string name)
    {
      string storageConnectionString =
        "DefaultEndpointsProtocol=https;AccountName=registry2;AccountKey=4Vr6myhUO7RAblnMaZnvxEhmU5K8PPhqAIvR/2HR8BANQzvgVpGuLEMnUU3v0GqQ9Ryqvvhv2SKdTI82nIsaWA==;EndpointSuffix=core.windows.net";

      CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
      CloudBlobClient serviceClient = account.CreateCloudBlobClient();

      var container = serviceClient.GetContainerReference("registrycontainer");
      CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);

      await blockBlob.UploadFromStreamAsync(file);
      return blockBlob.Uri.ToString();
    }

    public async Task CreateResource(CreateResourceRequest request)
    {
      await _resourceRepository.CreateResource(request);
    }

    public async Task<GetAllResourcesResult[]> GetAllResources()
    {
      return await _resourceRepository.GetAllResources();
    }
  }
}
