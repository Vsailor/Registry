using System;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class ResourceGroupService : IResourceGroupService
  {
    private IResourceGroupRepository _resourceGroupRepository;
    public ResourceGroupService()
    {
      _resourceGroupRepository = RegistryCommon.Instance.Container.Resolve<IResourceGroupRepository>();
    }

    public async Task<GetAllGroupsResult[]> GetAllResourceGroups()
    {
      return await _resourceGroupRepository.GetAllResourceGroups();
    }

    public async Task UpdateResourceGroup(Guid id, string name)
    {
      await _resourceGroupRepository.UpdateResourceGroup(id, name);
    }

    public async Task DeleteResourceGroup(Guid id)
    {
      await _resourceGroupRepository.DeleteGroup(id);
    }

    public async Task CreateResourceGroup(string name, string login)
    {
      await _resourceGroupRepository.CreateGroup(name, login);
    }
  }
}