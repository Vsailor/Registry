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

    public async Task<GetAllGroupsResult[]> GetAllThemes()
    {
      return await _resourceGroupRepository.GetAllResourceGroups();
    }

    public async Task UpdateTheme(Guid id, string name)
    {
      await _resourceGroupRepository.UpdateResourceGroup(id, name);
    }

    public async Task DeleteTheme(Guid id)
    {
      await _resourceGroupRepository.DeleteGroup(id);
    }

    public async Task CreateTheme(string name, CreateGroupResourceRequest[] request)
    {
      await _resourceGroupRepository.CreateGroup(name, request);
    }

    public async Task<string[]> GetUserThemes(string login)
    {
      return await _resourceGroupRepository.GetGroups(login);
    }
  }
}