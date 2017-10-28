using System;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Communication;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class ResourceGroupService : IResourceGroupService
  {
    private IResourceGroupRepository _resourceGroupRepository = RegistryCommon.Instance.Container.Resolve<IResourceGroupRepository>();

    public async Task<GetAllGroupsResult[]> GetAllResourceGroups()
    {
      return await _resourceGroupRepository.GetAllResourceGroupsAsync();
    }

    public async Task UpdateResourceGroup(Guid id, string name)
    {
      await _resourceGroupRepository.UpdateResourceGroupAsync(id, name);
    }

    public async Task DeleteResourceGroup(Guid id)
    {
      await _resourceGroupRepository.DeleteGroupAsync(id);
    }

    public async Task CreateResourceGroup(string name, string login)
    {
      await _resourceGroupRepository.CreateGroupAsync(name, login);
    }
  }
}